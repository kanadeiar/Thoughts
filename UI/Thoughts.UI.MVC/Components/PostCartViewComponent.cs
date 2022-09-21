namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class PostCartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Post post)
    {
        return View(post);
    }
}
