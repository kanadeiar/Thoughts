using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>
/// Категория (раздел)
/// </summary>
[Index(nameof(Name), IsUnique = true, Name = "NameIndex")]
public class Category : NamedEntity
{
    /// <summary>Статус категории</summary>
    [Required]
    [ForeignKey(nameof(StatusId))]
    public Status Status { get; set; } = null!;

    /// <summary> Внешний ключ статуса категории </summary>
    public int StatusId { get; set; }

    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public Category() { }

    public Category(string Name) => this.Name = Name;

    public override string ToString() => Name;
}
