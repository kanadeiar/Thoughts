namespace Thoughts.WebAPI.Clients.Files
{
    public interface IFileService
    {
        Task<bool> UploadLimitSizeFileAsync(Stream stream, string fileName, string contentType, CancellationToken token = default);

        bool UploadLimitSizeFile(Stream stream, string fileName, string contentType);

        Task<bool> UploadAnyFileAsync(Stream stream, string fileName, string contentType, CancellationToken token = default);

        bool UploadAnyFile(Stream stream, string fileName, string contentType);
    }
}
