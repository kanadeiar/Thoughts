namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class TagsListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ICollection<Tag> tags)
    {
        return View(tags);
    }
}
