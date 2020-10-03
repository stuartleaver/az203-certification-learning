using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace blob_storage
{
    class Program
    {
        private static CloudStorageAccount storageAccount;

        private static string connectionString = "<storage-account-connection-string>";

        static async Task Main(string[] args)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);

            Console.WriteLine("Block Blob Sample");
            await BasicStorageBlobOperationAsync();

            Console.WriteLine("\nPage Blob Sample");
            await BasicStoragePageBlobOperationsAsync();

            Console.WriteLine("\nDelete containers");
            await DeleteContainers();
        }

        private static async Task BasicStorageBlobOperationAsync()
        {
            var containerName= $"cat-images-{Guid.NewGuid().ToString().Substring(0, 8)}";

            var blobClient = storageAccount.CreateCloudBlobClient();

            var sasToken = GetAccountSASToken(storageAccount);

            var storageCredentials = new StorageCredentials(sasToken);

            Console.WriteLine();
            Console.WriteLine("Account SAS Signature: " + storageCredentials.SASSignature);
            Console.WriteLine("Account SAS Token: " + storageCredentials.SASToken);
            Console.WriteLine();

            var containerUri = GetContainerUri(containerName);

            var container = new CloudBlobContainer(containerUri, storageCredentials);

            Console.WriteLine("1. Creating Container");
            await container.CreateIfNotExistsAsync();

            Console.WriteLine("2. Getting list of files to upload");
            var files = GetFilesToUpload();

            Console.WriteLine("3. Uploading files");
            foreach(var file in files)
            {
                var blob = container.GetBlockBlobReference(Path.GetFileName(file));

                blob.Properties.ContentType = "image/png";

                await blob.UploadFromFileAsync(file);
            }

            await ListBlobsInContainer(container);

            var blobToDownload = container.GetBlockBlobReference(Path.GetFileName(files[3]));

            Console.WriteLine("5. Download Blob from {0}", blobToDownload.Uri.AbsoluteUri);
            //await blobToDownload.DownloadToFileAsync($"C:\\temp\\{files[3]}", FileMode.CreateNew);
        }

        private static async Task BasicStoragePageBlobOperationsAsync()
        {
            const string PageBlobName = "samplepageblob";

            var containerName= $"demo-pageblob-{Guid.NewGuid().ToString().Substring(0, 8)}";

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containerName);

             Console.WriteLine("1. Creating Container");
            await container.CreateIfNotExistsAsync();

            Console.WriteLine("2. Creating Page Blob");
            var pageBlob = container.GetPageBlobReference(PageBlobName);

            await pageBlob.CreateAsync(512 * 2);

            Console.WriteLine("3. Write to a Page Blob");
            var samplePageData = new byte[512];

            var random = new Random();

            random.NextBytes(samplePageData);

            await pageBlob.UploadFromByteArrayAsync(samplePageData, 0, samplePageData.Length);

            await ListBlobsInContainer(container);
        }

        private static async Task ListBlobsInContainer(CloudBlobContainer container)
        {
            BlobContinuationToken token = null;

            Console.WriteLine("4. List Blobs in Container");
            do
            {
                var resultSegment = await container.ListBlobsSegmentedAsync(token);

                token = resultSegment.ContinuationToken;

                foreach(var file in resultSegment.Results)
                {
                    Console.WriteLine($"File name: {file.Uri} File type: {file.GetType()}");
                }
            }
            while (token != null);
        }

        private static async Task DeleteContainers()
        {
            BlobContinuationToken token = null;

            do
            {
                var resultSegment = await storageAccount.CreateCloudBlobClient().ListContainersSegmentedAsync(token);

                token = resultSegment.ContinuationToken;

                foreach(var container in resultSegment.Results)
                {
                    Console.WriteLine($"Deleting container {container.Uri}");
                    await container.DeleteAsync();
                }
            }
            while (token != null);
        }

        private static Uri GetContainerUri(string containerName)
        {
            return storageAccount.CreateCloudBlobClient().GetContainerReference(containerName).Uri;
        }

        private static string GetAccountSASToken(CloudStorageAccount storageAccount)
        {
            // Create a new access policy for the account with the following properties:
            // Permissions: Read, Write, List, Create, Delete
            // ResourceType: Container
            // Expires in 24 hours
            // Protocols: HTTPS or HTTP (note that the storage emulator does not support HTTPS)
            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write | SharedAccessAccountPermissions.List | SharedAccessAccountPermissions.Create | SharedAccessAccountPermissions.Delete,
                Services = SharedAccessAccountServices.Blob,
                ResourceTypes = SharedAccessAccountResourceTypes.Container | SharedAccessAccountResourceTypes.Object,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Protocols = SharedAccessProtocol.HttpsOrHttp
            };

            // Create new storage credentials using the SAS token.
            string sasToken = storageAccount.GetSharedAccessSignature(policy);

            // Return the SASToken
            return sasToken;
        }

        private static string[] GetFilesToUpload()
        {
            var files = Directory.GetFiles(@"images\", "*.jpg");

            return files;
        }
    }
}
