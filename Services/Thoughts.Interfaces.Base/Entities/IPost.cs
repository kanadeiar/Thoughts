
using System.ComponentModel.DataAnnotations;

namespace Thoughts.Interfaces.Base.Entities;
/// <summary>
/// Запись (Пост)
/// </summary>
/// <typeparam name="TKye"></typeparam>
public interface IPost<TKye>:IEntity<TKye>
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

    /// <summary>Признак удалённой записи</summary>
    public bool IsDeleted { get; set; }
}
