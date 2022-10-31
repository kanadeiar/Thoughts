using System.Net.Http.Headers;

using Microsoft.Extensions.Logging;

using Thoughts.Interfaces.Base;

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

        public async Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType, CancellationToken token = default)
        {
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var form = new MultipartFormDataContent();
            form.Add(streamContent, fileName[..fileName.IndexOf('.')], fileName);

            var response = await _httpClient.PostAsync($"{WebApiControllersPath.FileUrl}/upload", form, token);

            return response.IsSuccessStatusCode;
        }

        public bool UploadFile(Stream stream, string fileName, string contentType) => UploadFileAsync(stream, fileName, contentType).GetAwaiter().GetResult();
    }
}
