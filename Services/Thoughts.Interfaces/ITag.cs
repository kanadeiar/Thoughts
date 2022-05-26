using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;

public interface ITag : INamedEntity
{
    public ICollection<IPost> Posts { get; set; }
}
