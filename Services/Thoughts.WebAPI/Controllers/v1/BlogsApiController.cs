using Thoughts.Interfaces;
using Newtonsoft;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Thoughts.Domain.Base.Entities;
using Thoughts.DAL.Entities;
using Post = Thoughts.Domain.Base.Entities.Post;

namespace Thoughts.WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/blogs/[action]")]
    [ApiController]
    public class BlogsApiController : ControllerBase
    {
        private readonly IBlogPostManager _BlogsManager;
        private readonly ILogger<BlogsApiController> _Logger;

        public BlogsApiController(IBlogPostManager BlogsManager, ILogger<BlogsApiController> Logger)
        {
            _BlogsManager = BlogsManager;
            _Logger = Logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts(CancellationToken ct)
        {
            var posts = await _BlogsManager.GetAllPostsAsync(ct);

            return Ok(posts);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int? id, CancellationToken ct)
        {
            var post = await _BlogsManager.GetPostAsync(id.Value, ct);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostsSkip(CancellationToken ct,int skip = 0, int take = 5)
        {
            var posts = await _BlogsManager.GetAllPostsSkipTakeAsync(skip, take, ct);
            if (posts == null)
                return NotFound();
            return Ok(posts);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostCount(CancellationToken ct)
        {
            var postsCount = await _BlogsManager.GetAllPostsCountAsync(ct);
            return Ok(postsCount);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostsPage(CancellationToken ct, int page = 1, int pageSize = 20)
        {
            var postsOnPage = await _BlogsManager.GetAllPostsPageAsync(page, pageSize, ct);
            if (postsOnPage == null)
                return NotFound();
            return Ok(postsOnPage);
        }
    }
}
