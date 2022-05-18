
using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    public interface ICategory<TKey> : INamedEntity<TKey>
    {

    }
    public interface ICategory : ICategory<int> { }
}
