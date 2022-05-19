using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>
/// Тег
/// </summary>
[Index(nameof(Name), IsUnique =true, Name = "NameIndex")]
public class Tag:Entity
{
    /// <summary>Название тега</summary>
    [Required, MinLength(3), MaxLength(60)]
    public string Name { get; set; } = null!;
    public ICollection<Post> Posts { get; set; }= new HashSet<Post>();

    public Tag() { }

    public Tag(string Name)=>this.Name = Name;

    public override string ToString() => Name;
}
