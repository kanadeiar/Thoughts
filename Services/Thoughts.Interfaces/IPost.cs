
using System.ComponentModel.DataAnnotations;

using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;
/// <summary>
/// Запись (Пост)
/// </summary>
/// <typeparam name="TKye"></typeparam>
public interface IPost<TKye> : IEntity<TKye>
{
    /// <summary>Статус записи</summary>
    [Required]
    public IStatus<TKye> Status { get; set; }

    /// <summary>Дата записи</summary>
    public DateTime Date { get; set; }

    /// <summary>Автор</summary>
    [Required]
    public IUser<TKye> User { get; set; }

    /// <summary>Заголовок записи</summary>
    [Required]
    public string Title { get; set; }

    /// <summary>Текст (тело) записи</summary>
    [Required, MinLength(20)]
    public string Body { get; set; }
    /// <summary>Категория к которой относится запись</summary>
    [Required]
    public ICategory<TKye> Category { get; set; }

    /// <summary>Список тегов относящихся к записи</summary>
    public ICollection<ITag<TKye>> Tags { get; set; }

    /// <summary>Список комментариев относящихся к записи</summary>
    public ICollection<IComment<TKye>> Comments { get; set; }

    /// <summary>Дата публикации</summary>
    public DateTime DatePublicatione { get; set; }

    /// <summary>Приложенные файлы</summary>
    public ICollection<IFile<TKye>> Files { get; set; }

    /// <summary>Адрес эл. почты</summary>
    [EmailAddress]
    public string Email { get; set; } = null!;
}
