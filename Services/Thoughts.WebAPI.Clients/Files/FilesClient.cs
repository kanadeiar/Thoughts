using Microsoft.Extensions.Logging;

using Thoughts.Interfaces.Base;

namespace Thoughts.WebAPI.Clients.Files
{
    public class FilesClient : IFilesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FilesClient> _logger;

        public FilesClient(HttpClient httpClient, ILogger<FilesClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> UploadFileAsync(Stream stream, CancellationToken token = default)
        {
            var content = new StreamContent(stream);    

            var response = await _httpClient.PostAsync($"{WebApiControllersPath.FilesUrl}/upload", content, token);

            return response.IsSuccessStatusCode;
        }

        public bool UploadFile(Stream stream) => UploadFileAsync(stream).GetAwaiter().GetResult();
    }
}
