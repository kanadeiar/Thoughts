
using System.ComponentModel.DataAnnotations;

using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;

/// <summary>
/// Комментарий
/// </summary>
public interface IComment : IEntity
{
    /// <summary>Дата комментария</summary>
    public DateTime Date { get; set; }

    /// <summary>Запись к которой принадлежит комментарий</summary>
    [Required]
    public IPost Post { get; set; }

    /// <summary>Автор комментария</summary>
    [Required]
    public IUser User { get; set; }

    /// <summary>Текст комментария</summary>
    [Required]
    public string Body { get; set; }

    /// <summary>Родительский комментарий</summary>
    public IComment? ParentComment { get; set; }

    /// <summary>Список дочерних комментариев</summary>
    public ICollection<IComment> ChildrenComment { get; set; }

    /// <summary>Признак удалённой записи</summary>
    public bool IsDeleted { get; set; }

}
