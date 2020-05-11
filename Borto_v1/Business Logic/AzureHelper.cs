using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class AzureHelper
    {
        #region Private Fields
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
        /// <param name="filePath"></param>
        /// <param name="videoName"></param>
        /// <returns></returns>
        public string UploadBlobsInChunks(string filePath, string videoName)
        {
            
            var containerClient = new BlobContainerClient(connectionString, "borto");
            containerClient.CreateIfNotExists();

            string uniqueness = Guid.NewGuid().ToString("N");

            string filename = videoName + uniqueness;
            var blockBlobClient = containerClient.GetBlockBlobClient(videoName + uniqueness);

            int blockSize = 1 * 1024 * 1024;//1 MB Block

            int offset = 0;

            int counter = 0;

            List<string> blockIds = new List<string>();

            using (var fs = File.OpenRead(filePath))
            {
                var bytesRemaining = fs.Length;
                do
                {
                    var dataToRead = Math.Min(bytesRemaining, blockSize);
                    byte[] data = new byte[dataToRead];
                    var dataRead = fs.Read(data, offset, (int)dataToRead);
                    bytesRemaining -= dataRead;
                    if (dataRead > 0)
                    {
                        var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(counter.ToString("d6")));
                        blockBlobClient.StageBlock(blockId, new MemoryStream(data));
                        //Block {0} uploaded successfully.
                        blockIds.Add(blockId);
                        counter++;
                    }
                }
                while (bytesRemaining > 0);
                //All blocks uploaded. Now committing block list.
                var headers = new BlobHttpHeaders()
                {
                    ContentType = "video/mp4"
                };
                blockBlobClient.CommitBlockList(blockIds, headers);
                return filename;
            }
        }
        public  void download_FromBlob(string filetoDownload,string filename , string pathFolder)
        {
            filename = "\\" + filename + ".mp4";
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("borto");
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filetoDownload);

            // provide the file download location below            
            Stream file = File.OpenWrite(pathFolder + filename);  

            cloudBlockBlob.DownloadToStream(file);

            file.Close();
        }
        #endregion
    }
}
