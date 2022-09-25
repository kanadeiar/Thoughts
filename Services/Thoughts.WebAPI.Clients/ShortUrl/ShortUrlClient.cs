using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Thoughts.Interfaces.Base;
using Thoughts.WebAPI.Clients.Base;

namespace Thoughts.WebAPI.Clients.ShortUrl
{
    public class ShortUrlClient: BaseClient, IShortUrlManager
    {
        public ShortUrlClient(IConfiguration Configuration):base(Configuration,WebApiControllersPath.ShortUrl)
        {

        }

        public async Task<string> AddUrlAsync(string Url, CancellationToken Cancel = default)
        {
            var response = await PostAsync<string>($"{WebApiControllersPath.ShortUrl}", Url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> DeleteUrlAsync(int Id, CancellationToken Cancel = default)
        {
            var response = await DeleteAsync($"{WebApiControllersPath.ShortUrl}/{Id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Uri?> GetUrlAsync(string Alias, CancellationToken Cancel = default)
        {
            var response = await GetAsync<Uri>($"{WebApiControllersPath.ShortUrl}?Alias={Alias}");
            return response;
        }

        public async Task<Uri?> GetUrlByIdAsync(int Id, CancellationToken Cancel = default)
        {
            var response = await GetAsync<Uri>($"{WebApiControllersPath.ShortUrl}/{Id}");
            return response;
        }

        public async Task<bool> UpdateUrlAsync(int Id, string Url, CancellationToken Cancel = default)
        {
            var response = await PostAsync<string>($"{WebApiControllersPath.ShortUrl}/{Id}", Url);
            return await response.Content.ReadAsAsync<bool>();
        }
    }
}
