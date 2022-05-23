
using System.ComponentModel.DataAnnotations;

using Thoughts.Domain.Base;

namespace Thoughts.Domain;

public class Status:Entity
{
    /// <summary>Наименование статуса</summary>
    [Required, MinLength(5)]
    public string Name { get; set; }= null!;

    public Status() { }

    public Status(string name) => Name = name;
    public override string ToString() => Name;
}
