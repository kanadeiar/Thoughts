using Thoughts.DAL;

namespace Thoughts.UI.MVC.Controllers;

public class BlogController : Controller 
{
    private readonly IBlogPostManager _postManager;
    private readonly IConfiguration _configuration;
    private readonly int _lengthText;
    private readonly ThoughtsDB _context;
    public BlogController(IBlogPostManager postManager, IConfiguration configuration, ThoughtsDB context)
    {
        _postManager = postManager;
        _configuration = configuration;
        _context = context;
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
        await InitViewBag();
        var post = await _postManager.GetPostAsync(id);
        var model = new BlogDetailsWebModel
        {
            Post = post,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BlogDetailsWebModel model, CancellationToken cancellation = default)
    {
        await InitViewBag();
        var post = model.Post;
        //if (ModelState.IsValid)
        {
            var test = await _postManager.ChangePostTitleAsync(post.Id, post.Title, cancellation);
            var test1 = await _postManager.ChangePostBodyAsync(post.Id, post.Body, cancellation);
            var test2 = await _postManager.ChangePostCategoryAsync(post.Id, post.Category?.Name, cancellation);
        }

        return RedirectToAction("Details", "Blog", new { Id = model.Post.Id });
    }

    private async Task InitViewBag()
    {
        ViewBag.Categoryes = new List<SelectListItem>()
        {
            new SelectListItem("--Не выбрано--", "")
        }.Concat(_context.Categories.Select(item => 
            new SelectListItem(item.Name, item.Id.ToString())))
            .ToList();
    }
}
