namespace Thoughts.WebAPI.Clients.Files
{
    public interface IFileService
    {
        Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType, CancellationToken token = default);

        bool UploadFile(Stream stream, string fileName, string contentType);
    }
}
