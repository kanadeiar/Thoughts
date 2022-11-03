namespace Thoughts.UI.MVC.Components;

[ViewComponent]
public class TagCloudViewComponent : ViewComponent
{
    private readonly IBlogPostManager _blogPostManager;
    public TagCloudViewComponent(IBlogPostManager blogPostManager) => _blogPostManager = blogPostManager;

    public IViewComponentResult Invoke()
    {
        var tags = _blogPostManager.GetMostPopularTags().Result;
        return View(tags.ToList());
    }
}
