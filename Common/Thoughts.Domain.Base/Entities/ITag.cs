
using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    public interface ITag<TKey> : INamedEntity<TKey>
    {

    }
    public interface ITag : ITag<int>, INamedEntity { }
}
