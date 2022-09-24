using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Thoughts.Interfaces.Base;

namespace Thoughts.WebAPI.Controllers
{
    [Route(WebApiControllersPath.ShortUrl)]
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
        public async Task<ActionResult<Uri>> GetUrl(string Alias)
        {
            var result = await _shortUrlManager.GetUrl(Alias);
            if (result is null)
                return BadRequest();

            return result;
        }

        // GET: api/url/10
        [HttpGet("{Id}")]
        public async Task<ActionResult<Uri>> GetUrlById(int Id)
        {
            var result = await _shortUrlManager.GetUrlById(Id);
            if (result is null)
                return BadRequest();

            return result;
        }


        // PUT api/url?Url=...
        [HttpPut]
        public async Task<ActionResult<string>> AddUrl(string Url)
        {
            var result = await _shortUrlManager.AddUrl(Url);
            if (String.IsNullOrEmpty(result))
                return BadRequest();

            //Ответ в формате api/url?Alias=447B38C52866B03C8129FD474502F558
            return $"{WebApiControllersPath.ShortUrl}?Alias={result}";
        }

        // DELETE api/url/10
        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteUrl(int Id)
        {
            var result= await _shortUrlManager.DeleteUrl(Id);
            return result ? result : BadRequest();
        }

        // POST api/url/10
        [HttpPost("{Id}")]
        public async Task<ActionResult<bool>> UpdateUrl(int Id, [FromBody] string Url)
        {
            var result=await _shortUrlManager.UpdateUrl(Id, Url);
            return result ? result : BadRequest();
        }
    }
}
