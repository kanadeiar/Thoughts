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
}
