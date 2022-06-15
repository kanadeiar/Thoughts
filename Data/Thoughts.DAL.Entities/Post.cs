using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

public class Post : Entity
{
    /// <summary>Заголовок записи</summary>
    [Required, MinLength(1)]
    public string Title { get; set; } = null!;

    /// <summary>Текст (тело) записи</summary>
    [Required, MinLength(20)]
    public string Body { get; set; } = null!;

    /// <summary>Дата записи</summary>
    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

    /// <summary>Дата публикации</summary>
    public DateTimeOffset? PublicationDate { get; set; }

    /// <summary>Статус записи</summary>
    [Required]
    public Status Status { get; set; }

    /// <summary>Категория к которой относится запись</summary>
    [Required]
    public Category Category { get; set; } = null!;

    /// <summary>Автор</summary>
    [Required]
    public User User { get; set; } = null!;

    /// <summary>Список тегов относящихся к записи</summary>
    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    /// <summary>Список комментариев относящихся к записи</summary>
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    public override string ToString() => $"{Date}, {User.NickName}: {Title}";
}
