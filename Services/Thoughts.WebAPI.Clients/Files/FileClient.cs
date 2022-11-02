using System.Net.Http.Headers;

using Microsoft.Extensions.Logging;

using Thoughts.Interfaces.Base;
using Thoughts.WebAPI.Clients.Tools;

namespace Thoughts.WebAPI.Clients.Files
{
    public class FileClient : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FileClient> _logger;

        public FileClient(HttpClient httpClient, ILogger<FileClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        #region IFileService implementation

        public async Task<bool> UploadLimitSizeFileAsync(Stream stream, string fileName, string contentType, CancellationToken token = default)
        {
            using var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            using var form = new MultipartFormDataContent();
            form.Add(streamContent, fileName[..fileName.IndexOf('.')], fileName);
            form.DeleteQuotesFromHeader("boundary");

            var response = await _httpClient.PostAsync($"{WebApiControllersPath.FileUrl}/upload", form, token);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UploadAnyFileAsync(Stream stream, string fileName, string contentType, CancellationToken token = default)
        {
            using var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            using var form = new MultipartFormDataContent();
            form.Add(streamContent, fileName[..fileName.IndexOf('.')], fileName);
            form.DeleteQuotesFromHeader("boundary");

            var response = await _httpClient.PostAsync($"{WebApiControllersPath.FileUrl}/uploadlarge", form, token);

            return response.IsSuccessStatusCode;
        }


        #region Sync versions

        public bool UploadLimitSizeFile(Stream stream, string fileName, string contentType) =>
           UploadLimitSizeFileAsync(stream, fileName, contentType).Result;

        public bool UploadAnyFile(Stream stream, string fileName, string contentType) =>
            UploadAnyFileAsync(stream, fileName, contentType).GetAwaiter().GetResult();  

        #endregion

        #endregion
    }
}
