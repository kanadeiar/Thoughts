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

    /// <summary>Дата публикации</summary>
    public DateTime DatePublicatione { get; set; }

    /// <summary>Приложенные файлы</summary>
    public ICollection<FileModel> Files { get; set; } = new HashSet<FileModel>();

    /// <summary>Адрес эл. почты</summary>
    [EmailAddress]
    public string Email { get; set; } = null!;
    public PostModel () { }

    public PostModel(StatusModel status, DateTime date, UserModel user,
        string title, string body, CategoryModel category, DateTime datePublicatione,
        ICollection<TagModel> tags, ICollection<CommentModel> comments, ICollection<FileModel> files, string email)
    {
        Status = status;
        Date = date;
        User = user;
        Title = title;
        Body = body;
        Category = category;
        Tags = tags;
        Comments = comments;
        DatePublicatione = datePublicatione;
        Files = files;
        Email = email;
    }

    public override string ToString() => $"{Date}, {User.NikName}: {Title}";

}
