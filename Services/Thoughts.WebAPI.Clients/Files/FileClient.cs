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

        public async Task<bool> UploadFileAsync(Stream stream, CancellationToken token = default)
        {
            var content = new StreamContent(stream);

            var response = await _httpClient.PostAsync($"{WebApiControllersPath.FileUrl}/upload", content, token);

            return response.IsSuccessStatusCode;
        }

        public bool UploadFile(Stream stream) => UploadFileAsync(stream).GetAwaiter().GetResult();
    }
}
