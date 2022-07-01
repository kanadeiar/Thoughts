namespace Thoughts.UI.MVC.TagHelpers;

/// <summary>
/// Таг-хелпер отображения html в сыром виде
/// </summary>
public class RawhtmlTagHelper : TagHelper
{
    private IHtmlHelper _htmlHelper { get; }
    public RawhtmlTagHelper(IHtmlHelper htmlHelper)
    {
        _htmlHelper = htmlHelper ?? throw new ArgumentNullException(nameof(htmlHelper));
    }

    /// <summary>
    /// Текст html
    /// </summary>
    public string Text { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        var text = _htmlHelper.Raw(Text);
        output.Content.SetHtmlContent(text);
    }
}
