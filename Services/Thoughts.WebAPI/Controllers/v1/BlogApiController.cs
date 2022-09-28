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
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BlogApiController : Controller
    {
        private readonly IBlogPostManager _blogPostManager;
        public BlogApiController(IBlogPostManager blogPostManager)
        {
            _blogPostManager = blogPostManager;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<Post> GetById(int id)
        {
            var post = await _blogPostManager.GetPostAsync(id);

            return post;
        }
    }
}
