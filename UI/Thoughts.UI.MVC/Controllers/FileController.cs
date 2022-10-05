using Microsoft.AspNetCore.Mvc;

namespace Thoughts.UI.MVC.Controllers;
public class FileController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
