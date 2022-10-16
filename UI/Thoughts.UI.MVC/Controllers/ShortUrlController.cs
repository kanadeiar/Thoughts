using Microsoft.AspNetCore.Mvc;

using Thoughts.Interfaces.Base;

namespace Thoughts.UI.MVC.Controllers
{
    [Route("url")]
    public class ShortUrlController : Controller
    {
        private readonly IShortUrlManager _shortUrlManager;

        public ShortUrlController(IShortUrlManager ShortUrlManager)
        {
            _shortUrlManager = ShortUrlManager;
        }

        [Route("url/{Alias}")]
        [HttpGet("{Alias}")]
        public async Task<IActionResult> GetUrl(string Alias)
        {
            var url=await _shortUrlManager.GetUrlAsync(Alias);
            if(url is null)
                return NotFound();
            return Redirect(url.AbsoluteUri);
        }

        [HttpPost]
        public async Task<IActionResult> AddUrl(string Url)
        {
            var result = await _shortUrlManager.AddUrlAsync(Url);
            if (String.IsNullOrEmpty(result))
                return BadRequest();
            ShortUrlWebModel shortUrlWebModel = new()
            {
                Alias = result,
                OriginalUrl = Url
            };
            return View(shortUrlWebModel);
        }

        [Route("Test")]
        public IActionResult Test()
        {
            return View();
        }
    }
}
