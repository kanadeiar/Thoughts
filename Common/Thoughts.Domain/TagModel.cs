using System.ComponentModel.DataAnnotations;

using Thoughts.Domain.Base;

namespace Thoughts.Domain;

/// <summary>
/// Тег
/// </summary>
public class TagModel: NamedEntityModel
{
    public ICollection<PostModel> Posts { get; set; }= new HashSet<PostModel>();

    public TagModel() { }

    public TagModel(string Name)=>this.Name = Name;

    public override string ToString() => Name;
}
