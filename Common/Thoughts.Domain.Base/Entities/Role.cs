namespace Thoughts.Domain.Base.Entities;

public class Role : NamedEntityModel
{
    public ICollection<User> Users { get; set; } = new HashSet<User>();

    public Role() { }

    public Role(string name) => Name = name;

    public override string ToString() => Name;
}
