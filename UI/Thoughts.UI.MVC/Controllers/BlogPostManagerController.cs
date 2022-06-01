using Microsoft.AspNetCore.Mvc;

using Thoughts.Interfaces;

namespace Thoughts.UI.MVC.Controllers;

public class BlogPostManagerController : Controller 
{
    private readonly IBlogPostManager manager;
    private readonly ILogger logger;

    public BlogPostManagerController(IBlogPostManager Manager, ILogger Logger)
    {
        manager = Manager;
        logger = Logger;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await manager.GetAllPostsAsync();
        return View(posts);
    }
}
