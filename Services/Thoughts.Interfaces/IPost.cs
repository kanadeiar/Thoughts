
using System.ComponentModel.DataAnnotations;

using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;
/// <summary>
/// Запись (Пост)
/// </summary>
public interface IPost : IEntity
{
    /// <summary>Статус записи</summary>
    [Required]
    public IStatus Status { get; set; }

    /// <summary>Дата записи</summary>
    public DateTime Date { get; set; }

    /// <summary>Автор</summary>
    [Required]
    public IUser User { get; set; }

    /// <summary>Заголовок записи</summary>
    [Required]
    public string Title { get; set; }

    /// <summary>Текст (тело) записи</summary>
    [Required, MinLength(20)]
    public string Body { get; set; }

    /// <summary>Категория к которой относится запись</summary>
    [Required]
    public ICategory Category { get; set; }

    /// <summary>Список тегов относящихся к записи</summary>
    public ICollection<ITag> Tags { get; set; }

    /// <summary>Список комментариев относящихся к записи</summary>
    public ICollection<IComment> Comments { get; set; }

    /// <summary>Дата публикации</summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>Приложенные файлы</summary>
    public ICollection<IFile> Files { get; set; }
}
