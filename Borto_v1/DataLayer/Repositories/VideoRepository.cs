using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class VideoRepository : IRepository<Video>
    {
        private PlayerContext db;

        public VideoRepository(PlayerContext context)
        {
            this.db = context;
        }

        public void Create(Video item)
        {
            db.Videos.Add(item);
        }

        public void Delete(int id)
        {
            Video video = db.Videos.Find(id);
            if (video != null)
                db.Videos.Remove(video);
        }

        public IEnumerable<Video> Find(Func<Video, bool> predicate)
        {
            return db.Videos.Where(predicate).ToList();
        }

        public Video Get(int id)
        {
            return db.Videos.Find(id);
        }

        public IEnumerable<Video> GetAll()
        {
            return db.Videos.Include(c => c.User);
        }

        public void Update(Video item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        /// <summary>
        /// Add video to the azure blob storage 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="videoName"></param>
        /// <returns></returns>
        public string UploadBlobsInChunks(string filePath,string videoName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["StorageAccount"].ConnectionString;
            var containerClient = new BlobContainerClient(connectionString, "borto");
            containerClient.CreateIfNotExists();

            string uniqueness = Guid.NewGuid().ToString("N");

            string serverpath = "https://robinbook.blob.core.windows.net/borto/" + videoName + uniqueness;
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
                return serverpath;
            }
        }

    }
}
