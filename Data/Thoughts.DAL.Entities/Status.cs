using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

[Index(nameof(Name), IsUnique = true, Name = "NameIndex")]
public class Status : NamedEntity
{
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public Status() { }

    public Status(string name) => Name = name;

    public override string ToString() => Name;
}
