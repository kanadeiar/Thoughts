
using System.ComponentModel.DataAnnotations;

using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;

/// <summary>
/// Комментарий
/// </summary>
/// <typeparam name="TKye"></typeparam>
public interface IComment<TKye> : IEntity<TKye>
{
    /// <summary>Дата комментария</summary>
    public DateTime Date { get; set; }

    /// <summary>Запись к которой принадлежит комментарий</summary>
    [Required]
    public IPost<TKye> Post { get; set; }

    /// <summary>Автор комментария</summary>
    [Required]
    public IUser<TKye> User { get; set; }

    /// <summary>Текст комментария</summary>
    [Required]
    public string Body { get; set; }

    /// <summary>Родительский комментарий</summary>
    public IComment<TKye>? ParentComment { get; set; }

    /// <summary>Список дочерних комментариев</summary>
    public ICollection<IComment<TKye>> ChildrenComment { get; set; }

    /// <summary>Признак удалённой записи</summary>
    public bool IsDeleted { get; set; }

}
