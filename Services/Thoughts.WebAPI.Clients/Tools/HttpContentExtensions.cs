namespace Thoughts.WebAPI.Clients.Tools
{
    internal static class HttpContentExtensions
    {
        public static void DeleteQuotesFromHeader(this HttpContent httpContent, string headerName)
        {
            if (httpContent is null || string.IsNullOrEmpty(headerName)) return;

            var header = httpContent.Headers.ContentType.Parameters
                .FirstOrDefault(p => string.Equals(p.Name, headerName));

            if (header is null)
                throw new ArgumentNullException($"{headerName} doesn't exist in http content");

            header.Value = header.Value.Replace("\"", string.Empty);
        }
    }
}
