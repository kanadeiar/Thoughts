using System.ComponentModel.DataAnnotations;

using Thoughts.Domain.Base;

namespace Thoughts.Domain;

public  class PostModel : EntityModel
{
    /// <summary>Статус записи</summary>
    [Required]
    public StatusModel Status { get; set; } = null!;

    /// <summary>Дата записи</summary>
    public DateTime Date { get; set; }

    /// <summary>Автор</summary>
    [Required]
    public UserModel User { get; set; }=null!;

    /// <summary>Заголовок записи</summary>
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>Текст (тело) записи</summary>
    [Required, MinLength(20)]
    public string Body { get; set; } = null!;
    /// <summary>Категория к которой относится запись</summary>
    [Required] 
    public CategoryModel Category { get; set; } = null!;

    /// <summary>Список тегов относящихся к записи</summary>
    public ICollection<TagModel> Tags { get; set; }= new HashSet<TagModel>();

    /// <summary>Список комментариев относящихся к записи</summary>
    public ICollection<CommentModel> Comments { get; set; }= new HashSet<CommentModel>();

    /// <summary>Признак удалённой записи</summary>
    public bool IsDeleted { get; set; }

    public PostModel () { }

    public PostModel (StatusModel status, DateTime date, UserModel user, 
        string title, string body, CategoryModel category, bool isDeleted ) 
    { 
        Status = status;
        Date = date;
        User = user;
        Title = title;
        Body = body;
        Category = category;
        IsDeleted = isDeleted;           
    }

    public override string ToString() => $"{Date}, {User.NikName}: {Title}";

}
