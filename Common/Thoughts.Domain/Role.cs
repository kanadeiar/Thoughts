using System.ComponentModel.DataAnnotations;


using Thoughts.Domain.Base;

namespace Thoughts.Domain;


public class Role : Entity
{
    [Required]
    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new HashSet<User>();
    public Role() { }

    public Role(string name) => Name = name;

    public override string ToString() => Name;

}
