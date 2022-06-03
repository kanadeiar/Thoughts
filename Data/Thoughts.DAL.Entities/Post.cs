using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

public class Post : Entity
{
    public int StatusId { get; set; }

    /// <summary>Статус записи</summary>
    [Required]
    [ForeignKey(nameof(StatusId))]
    public Status Status { get; set; } = null!;

    /// <summary>Дата записи</summary>
    public DateTime Date { get; set; } = DateTime.Now; // todo: заменить тип на DateTimeOffset и переделать миграцию

    /// <summary>Автор</summary>
    [Required]
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    /// <summary> Внешний ключ для пользователя </summary>
    public string UserId { get; set; }

    /// <summary>Заголовок записи</summary>
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>Текст (тело) записи</summary>
    [Required, MinLength(20)]
    public string Body { get; set; } = null!;

    /// <summary>Категория к которой относится запись</summary>
    [Required]
    [ForeignKey(nameof(CategoryName))]
    public Category Category { get; set; } = null!;

    /// <summary> Внешний ключ для категории </summary>
    public string CategoryName { get; set; }

    /// <summary>Список тегов относящихся к записи</summary>
    [ForeignKey(nameof(TagsName))]
    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    public ICollection<string> TagsName { get; set; }

    /// <summary>Список комментариев относящихся к записи</summary>
    [ForeignKey(nameof(CommentsId))]
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    /// <summary> Внешний ключ для комментариев </summary>
    public ICollection<int> CommentsId { get; set; }

    /// <summary>Дата публикации</summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>Приложенные файлы</summary>
    public ICollection<File> Files { get; set; } = new HashSet<File>();

    /// <summary>Адрес эл. почты</summary>
    [EmailAddress]
    public string Email { get; set; } = null!;

    public Post() { }

    public Post(Status status, DateTime date, User user,
        string title, string body, Category category, DateTime publicationDate,
        ICollection<Tag> tags, ICollection<Comment> comments, ICollection<File> files, string email)
    {
        Status = status;
        Date = date;
        User = user;
        Title = title;
        Body = body;
        Category = category;
        Tags = tags;
        Comments = comments;
        PublicationDate = publicationDate;
        Files = files;
        Email = email;
    }

    public override string ToString() => $"{Date}, {User.NickName}: {Title}";

}
