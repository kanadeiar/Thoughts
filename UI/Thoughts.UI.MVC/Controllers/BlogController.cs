namespace Thoughts.UI.MVC.Controllers;

public class BlogController : Controller 
{
    private readonly IBlogPostManager _postManager;
    private readonly IConfiguration _configuration;
    private readonly int _lengthText;

    public BlogController(IBlogPostManager postManager, IConfiguration configuration)
    {
        _postManager = postManager;
        _configuration = configuration;
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
}
