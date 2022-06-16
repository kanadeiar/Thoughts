using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>Тег</summary>
[Index(nameof(Name), IsUnique = true, Name = "NameIndex")]
public class Tag : NamedEntity
{
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public override string ToString() => Name;
}
