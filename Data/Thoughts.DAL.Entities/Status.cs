
using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

[Index(nameof(Name), IsUnique = true, Name = "NameIndex")]
public class Status:Entity
{
    /// <summary>Наименование статуса</summary>
    public string Name { get; set; }= null!;

    public Status() { }

    public Status(string name) => Name = name;
    public override string ToString() => Name;
}
