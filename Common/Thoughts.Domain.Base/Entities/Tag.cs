namespace Thoughts.Domain.Base.Entities;

/// <summary>Ключевое слово</summary>
public class Tag : NamedEntityModel
{
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public override string ToString() => Name;
}
