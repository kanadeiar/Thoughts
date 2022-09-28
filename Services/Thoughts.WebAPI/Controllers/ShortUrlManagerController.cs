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
            var result = await _shortUrlManager.GetUrlAsync(Alias);
            if (result is null)
                return BadRequest();

            return result;
        }

        // GET: api/url/10
        [HttpGet("{Id}")]
        public async Task<ActionResult<Uri>> GetUrlById(int Id)
        {
            var result = await _shortUrlManager.GetUrlByIdAsync(Id);
            if (result is null)
                return BadRequest();

            return result;
        }


        // POST api/url
        [HttpPost]
        public async Task<IActionResult> AddUrl([FromBody]string Url)
        {
            // 267FC37D9A60075A69DDFC466AAD4FB6
            var result = await _shortUrlManager.AddUrlAsync(Url);
            if (string.IsNullOrEmpty(result))
                return BadRequest();

            //return CreatedAtAction(nameof(GetUrlById), new { Id = id }, )
            return Ok(result);
        }

        // DELETE api/url/10
        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteUrl(int Id)
        {
            var result= await _shortUrlManager.DeleteUrlAsync(Id);
            return result ? result : BadRequest();
        }

        // POST api/url/10
        [HttpPost("{Id}")]
        public async Task<ActionResult<bool>> UpdateUrl(int Id, [FromBody] string Url)
        {
            var result=await _shortUrlManager.UpdateUrlAsync(Id, Url);
            return result ? result : BadRequest();
        }
    }
}
