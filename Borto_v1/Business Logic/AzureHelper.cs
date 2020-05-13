
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
namespace Borto_v1
{
    public class AzureHelper
    {
        #region Private Fields
        private Stream file;
        private string connectionString;
        #endregion


        #region
        public AzureHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["StorageAccount"].ConnectionString;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Add video to the azure blob storage 
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="videoName"></param>
        /// <returns></returns>
        public string upload_ToBlob(string fileToUpload, string videoName)
        {
            string uniqueness = Guid.NewGuid().ToString("N");

            string filename = videoName + uniqueness;
            
            file = new FileStream(fileToUpload, FileMode.Open);

            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("borto");

            //checking the container exists or not  
            if (container.CreateIfNotExists())
            {
                container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess =
                  BlobContainerPublicAccessType.Blob
                });
            }
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename);

            cloudBlockBlob.Properties.ContentType = "video/mp4";

            cloudBlockBlob.UploadFromStream(file); // << Uploading the file to the blob >>  

            file.Close();

            return filename;

        }

        /// <summary>
        /// Download file to Azure 
        /// </summary>
        /// <param name="filetoDownload"></param>
        /// <param name="filename"></param>
        /// <param name="pathFolder"></param>
        public  void download_FromBlob(string filetoDownload,string filename , string pathFolder)
        {
            filename = "\\" + filename + ".mp4";
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("borto");
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filetoDownload);

            // provide the file download location below            
            file = File.OpenWrite(pathFolder + filename);  

            cloudBlockBlob.DownloadToStream(file);
            file.Close();
        }
        #endregion
    }
}
