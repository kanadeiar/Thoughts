using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Thoughts.UI.MVC.WebModels;

/// <summary> Вебмодель отображения детальных данных по одному элементу </summary>
public class BlogDetailsWebModel
{
    /// <summary> Данные по элементу </summary>
    [ValidateNever]
    public Post Post { get; set; }

    public int PostId { get; set; }

    public string UserId { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public string CategoryName { get; set; }
}
