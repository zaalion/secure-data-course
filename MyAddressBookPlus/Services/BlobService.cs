using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MyAddressBookPlus.Services
{
    public class BlobService
    {
        public string UploadPictureToBlob(string imagesPath, string filename)
        {
            string storageConnectionString = ConfigurationManager.AppSettings["storageconnectionstring"];
            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                // Create a container called 'photos' if not exist
                var cloudBlobContainer = cloudBlobClient.GetContainerReference("photos");
                cloudBlobContainer.CreateIfNotExists();

                // Set the permissions so the blobs are public. 
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };

                cloudBlobContainer.SetPermissions(permissions);

                // Create a unique filename to upload to a blob.
                var sourceFile = Path.Combine(imagesPath, filename);

                // Get a reference to the blob address, then upload the file to the blob.
                // Use the value of localFileName for the blob name.
                var uniqueFileName = $"{Guid.NewGuid()}-{filename}";
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(uniqueFileName);
                cloudBlockBlob.UploadFromFile(sourceFile);

                return uniqueFileName;
            }

            return string.Empty;
        }
    }
}