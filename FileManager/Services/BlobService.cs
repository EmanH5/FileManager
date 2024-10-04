using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class BlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "filesblob";

    public BlobService(IConfiguration configuration)
    {
        _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
    }

    public async Task UploadFileAsync(IFormFile file)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(file.FileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        }
    }

    public async Task<List<string>> ListFilesAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var items = new List<string>();

        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            items.Add(blobItem.Name);
        }

        return items;
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        var response = await blobClient.DownloadAsync();
        return response.Value.Content;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }
}
