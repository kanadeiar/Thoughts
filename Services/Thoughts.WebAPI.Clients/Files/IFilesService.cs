namespace Thoughts.WebAPI.Clients.Files
{
    public interface IFilesService
    {
        Task<bool> UploadFileAsync(Stream stream, CancellationToken token = default);

        bool UploadFile(Stream stream);
    }
}
