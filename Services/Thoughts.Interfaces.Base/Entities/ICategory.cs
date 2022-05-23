using System.ComponentModel.DataAnnotations;

namespace Thoughts.Interfaces.Base.Entities;
/// <summary>
/// Категория
/// </summary>
/// <typeparam name="TKye"></typeparam>
public interface ICategory<TKye>:INamedEntity<TKye>
{
    /// <summary>Статус категории</summary>
    [Required]
    public IStatus<TKye> Status { get; set; }

    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<IPost<TKye>> Posts { get; set; }
}
