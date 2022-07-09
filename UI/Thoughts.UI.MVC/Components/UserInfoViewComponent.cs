namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class UserInfoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
