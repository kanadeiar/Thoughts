namespace Thoughts.UI.MVC.TagHelpers;

/// <summary>
/// Таг-хелпер отображения html в сыром виде
/// </summary>
public class RawhtmlTagHelper : TagHelper
{
    public RawhtmlTagHelper()
    {
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var c = (await output.GetChildContentAsync()).GetContent();
        var text = HttpUtility.HtmlDecode(c);
        output.Content.SetHtmlContent(text);
    }
}
