using System.ComponentModel.DataAnnotations;


using Thoughts.Domain.Base;

namespace Thoughts.Domain;

/// <summary>
/// Категория (раздел)
/// </summary>

public class Category:Entity
{
    /// <summary>Статус категории</summary>
    [Required]
    public Status Status { get; set; } = null!;

    /// <summary>Наименование категории (раздела)</summary>      
    [Required, MinLength(3)]
    public string Name { get; set; } = null!;

    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<Post> Posts { get; set; }=new HashSet<Post>();

    public Category() { }

    public Category(string Name)=>this.Name = Name;

    public override string ToString() => Name;
}
