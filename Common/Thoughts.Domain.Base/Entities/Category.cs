using System.ComponentModel.DataAnnotations;

namespace Thoughts.Domain.Base.Entities;

/// <summary> Категория (раздел)</summary>
public class Category : NamedEntityModel
{
    /// <summary>Статус категории</summary>
    [Required]
    public Status Status { get; set; } = null!;

    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public Category() { }

    public Category(string Name) => this.Name = Name;

    public override string ToString() => Name;
}
