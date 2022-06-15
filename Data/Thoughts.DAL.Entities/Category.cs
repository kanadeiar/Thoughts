using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>Категория (раздел)</summary>
[Index(nameof(Name), IsUnique = true, Name = "NameIndex")]
public class Category : NamedEntity
{
    /// <summary>Статус категории</summary>
    [Required]
    public Status Status { get; set; }

    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public override string ToString() => Name;
}
