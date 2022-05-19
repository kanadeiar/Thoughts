using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>
/// Категория (раздел)
/// </summary>
[Index(nameof(Name),IsUnique =true, Name ="NameIndex")]
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
