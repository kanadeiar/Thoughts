using System.ComponentModel.DataAnnotations;

namespace Thoughts.Domain.Base.Entities;

/// <summary> Категория (раздел)</summary>
public class Category : NamedEntityModel
{
    /// <summary>Статус категории</summary>
    [Required]
    public Status Status { get; set; }

    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<int> Posts { get; set; } = new HashSet<int>();

    public override string ToString() => Name;
}
