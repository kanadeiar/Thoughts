using Microsoft.AspNetCore.Mvc;

using Thoughts.Interfaces;

namespace Thoughts.UI.MVC.Controllers;

public class BlogPostManagerController : Controller 
{
    private readonly IBlogPostManager manager;
    private readonly ILogger logger;

    /// <summary>
    /// Конструктор контроллера
    /// </summary>
    /// <param name="Manager"> Интерфейс реализуемого сервиса </param>
    /// <param name="Logger"> Логгер </param>
    public BlogPostManagerController(IBlogPostManager Manager, ILogger Logger)
    {
        manager = Manager;
        logger = Logger;
    }

    /// <summary>
    /// Запрос контроллера на получение всех постов
    /// </summary>
    /// <returns> Возврат всех постов в ViewResult </returns>
    public async Task<IActionResult> Index()
    {
        var posts = await manager.GetAllPostsAsync();
        return View(posts);
    }
}
