namespace Thoughts.Domain.Base.Entities;

public class Role : NamedEntityModel
{
    public ICollection<User> Users { get; set; } = new HashSet<User>();

    public override string ToString() => Name;
}
