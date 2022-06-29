using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thoughts.Domain.Base.Entities;

public class Post : EntityModel
{
    /// <summary>Статус записи</summary>
    [Required]
    public Status Status { get; set; }

    /// <summary>Дата записи</summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>Автор</summary>
    [Required]
    public User User { get; set; } = null!;

    /// <summary>Заголовок записи</summary>
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>Текст (тело) записи</summary>
    [Required, MinLength(20)]
    public string Body { get; set; } = null!;

    /// <summary>Категория к которой относится запись</summary>
    [Required]
    public Category Category { get; set; } = null!;

    /// <summary>Список тегов относящихся к записи</summary>
    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    /// <summary>Список комментариев относящихся к записи</summary>
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    /// <summary>Дата публикации</summary>
    public DateTimeOffset? PublicationsDate { get; set; }

    /// <summary>Приложенные файлы</summary>
    public ICollection<FileModel> Files { get; set; } = new HashSet<FileModel>();

    public override string ToString() => $"{Date}, {User.NickName}: {Title}";
}
