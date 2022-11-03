using AutoMapper;

using Microsoft.CodeAnalysis.FlowAnalysis;

using Thoughts.DAL;
using Thoughts.UI.MVC.Infrastructure.AutoMapper;

namespace Thoughts.UI.MVC.Controllers;

public class BlogController : Controller 
{
    private readonly IBlogPostManager _postManager;
    private readonly IConfiguration _configuration;
    private IMapper _mapper;
    private readonly int _lengthText;
    private readonly ThoughtsDB _context;
    public BlogController(IBlogPostManager postManager, IConfiguration configuration, ThoughtsDB context, IMapper mapper)
    {
        _postManager = postManager;
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
        _lengthText = _configuration.GetValue<int>("LengthTextOnHomeView");
    }

    /// <summary>
    /// Запрос контроллера на получение всех постов
    /// </summary>
    /// <returns> Возврат всех постов в ViewResult </returns>
    public async Task<IActionResult> Index()
    {
        var posts = (await _postManager.GetAllPostsAsync()).ToArray();
        StringTools.CutBodyTextInPosts(posts, _lengthText);
        var model = new BlogIndexWebModel
        {
            Posts = posts,
        };
        return View(model);
    }

    /// <summary>
    /// Данные детальные по одному элемпенту
    /// </summary>
    /// <param name="id">Идентификатор элемента</param>
    /// <returns>Вьюха с детальными данными по посту</returns>
    public async Task<IActionResult> Details(int id)
    {
        var post = await _postManager.GetPostAsync(id);
        var model = new BlogDetailsWebModel
        {
            Post = post,
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _postManager.GetPostAsync(id);
        var currentUser = await _context.Users.FirstOrDefaultAsync();//TODO Get current userId from identity DB
        var model = new BlogDetailsWebModel
        {
            Post = post,
            UserId = currentUser != null ? currentUser.Id : Guid.NewGuid().ToString() //TODO Get current userId from identity DB
        };
        var viewModel = _mapper.Map(post ?? new Post(), model);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BlogDetailsWebModel model, CancellationToken cancellation = default)
    {
        var post = new Post(); 
        if (ModelState.IsValid)
        {
            if (model.PostId > 0)
            {
                await _postManager.ChangePostTitleAsync(model.PostId, model.Title, cancellation);
                await _postManager.ChangePostBodyAsync(model.PostId, model.Body, cancellation);
                await _postManager.ChangePostCategoryAsync(model.PostId, model.CategoryName, cancellation);
                return RedirectToAction("Details", "Blog", new { Id = model.PostId });
            }

            post = await _postManager.CreatePostAsync(model.Title, model.Body, model.UserId, model.CategoryName, cancellation);
        }

        return RedirectToAction("Details", "Blog", new { post.Id });
    }

    [Route("[controller]/tag/{tagName}")]
    public async Task<IActionResult> GetPostByTag(string tagName, CancellationToken cancellation)
    {
        var posts = await _postManager.GetPostsByTag(tagName, cancellation);
        var viewModel = new BlogIndexWebModel
        {
            Posts = posts,
        };
        return View("Index", viewModel);
    }
    public async Task<IActionResult> TypeaheadQuery(string query)
    {
        var categories = _context.Categories.Where(item => item.Name.StartsWith(query)).ToList();
        return Json(categories.Select(item => new
        {
            item.Name
        }));
    }
}
