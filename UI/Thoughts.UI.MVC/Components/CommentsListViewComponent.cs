namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class CommentsListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ICollection<Comment> comments)
    {
        return View(comments);
    }
}
