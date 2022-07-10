namespace Thoughts.Domain.Base.Entities;

/// <summary>Ключевое слово</summary>
public class Tag : NamedEntityModel
{
    public ICollection<int> Posts { get; set; } = new HashSet<int>();

    public override string ToString() => Name;
}
