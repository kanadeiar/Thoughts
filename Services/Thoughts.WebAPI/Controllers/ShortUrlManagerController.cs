using Microsoft.AspNetCore.Mvc;

using Thoughts.Interfaces.Base;

namespace Thoughts.WebAPI.Controllers
{
    [Route("api/url")]
    [ApiController]
    public class ShortUrlManagerController : ControllerBase
    {
        private readonly IShortUrlManager _shortUrlManager;

        public ShortUrlManagerController(IShortUrlManager ShortUrlManager)
        {
            _shortUrlManager = ShortUrlManager;
        }

          // GET: api/url?Alias=...
        [HttpGet]
        public Task<Uri?> GetUrl(string Alias) => _shortUrlManager.GetUrl(Alias);

        // GET: api/url/10
        [HttpGet("{Id}")]
        public Task<Uri?> GetUrlById(int Id) => _shortUrlManager.GetUrlById(Id);

        // PUT api/url
        [HttpPut]
        public Task<string> AddUrl( string Url)
        {
            _shortUrlManager.AddUrl(Url);
        }

        // DELETE api/url/10
        [HttpDelete("{Id}")]
        public Task<bool> DeleteUrl(int Id) => _shortUrlManager.DeleteUrl(Id);

        // POST api/url/10
        [HttpPost("{Id}")]
        public Task<bool> UpdateUrl(int Id, [FromBody] string Url) => _shortUrlManager.UpdateUrl(Id, Url);
    }
}
