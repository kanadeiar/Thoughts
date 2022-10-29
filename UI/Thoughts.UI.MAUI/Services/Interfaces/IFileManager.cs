namespace Thoughts.UI.MAUI.Services.Interfaces
{
    public interface IFileManager
    {
        Task<bool> UploadFileAsync(FileResult file, CancellationToken token = default);

        bool UploadFile(FileResult file);
    }
}
