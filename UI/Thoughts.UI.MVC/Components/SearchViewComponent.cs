namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class SearchViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
