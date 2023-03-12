using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WebApp.Interfaces;

namespace WebApp.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<BlobService> _logger;
        private readonly string blobStorageconnection;
        private readonly string blobContainerName;
        public BlobService(BlobServiceClient blobServiceClient, ILogger<BlobService> logger)
        {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
            blobStorageconnection = "DefaultEndpointsProtocol=https;AccountName=reenbittesttask;AccountKey=vK6aF190Ly7Q2t1I2M1pwmzwheusYnT3WilBH4XSCjQV4THEhMA5cspV0tuhcyubhYxS7aoIAO7h+AStanvFIw==;EndpointSuffix=core.windows.net";
            blobContainerName = "fordocs";        
        }

        public async Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream)
        {
            try
            {
                var container = new BlobContainerClient(blobStorageconnection, blobContainerName);
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                var blob = container.GetBlobClient(strFileName);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contecntType });
                var urlString = blob.Uri.ToString();
                return urlString;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                throw;
            }
        }

    }
}
