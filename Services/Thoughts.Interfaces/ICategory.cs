using System.ComponentModel.DataAnnotations;

using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;

/// <summary>
/// Категория
/// </summary>
public interface ICategory : INamedEntity
{
    /// <summary>Статус категории</summary>
    [Required]
    public IStatus Status { get; set; }

    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<IPost> Posts { get; set; }
}
