namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class DetailsCartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Post post)
    {
        return View(post);
    }
}
