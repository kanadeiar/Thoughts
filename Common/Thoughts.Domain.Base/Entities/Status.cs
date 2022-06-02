namespace Thoughts.Domain.Base.Entities;

public class Status : NamedEntityModel
{
    public Status() { }

    public Status(string name) => Name = name;

    public override string ToString() => Name;
}
