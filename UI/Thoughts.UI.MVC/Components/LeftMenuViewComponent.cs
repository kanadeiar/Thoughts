namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class LeftMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
