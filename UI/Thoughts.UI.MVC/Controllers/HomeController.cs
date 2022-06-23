namespace Thoughts.UI.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IBlogPostManager _postManager;
    private readonly IConfiguration _configuration;
    private readonly int _countOnHomeView;
    private readonly int _lengthText;

    public HomeController(IBlogPostManager postManager, IConfiguration configuration)
    {
        _postManager = postManager;
        _configuration = configuration;
        _countOnHomeView = _configuration.GetValue<int>("CountPostsOnHomeView");
        _lengthText = _configuration.GetValue<int>("LengthTextOnHomeView");
    }

    public async Task<IActionResult> Index()
    {
        var countPosts = await _postManager.GetAllPostsCountAsync();
        var skipPosts = (countPosts < _countOnHomeView) ? _countOnHomeView : countPosts - _countOnHomeView;
        var posts = (await _postManager.GetAllPostsSkipTakeAsync(skipPosts, _countOnHomeView)).OrderByDescending(x => x.Date).ToArray();
        StringTools.CutBodyTextInPosts(posts, _lengthText);
        var model = new HomeIndexWebModel
        {
            Posts = posts,
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
