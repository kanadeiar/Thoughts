
using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    /// <summary>Интерфейс сущности тэга</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public interface ITag<TKey> : INamedEntity<TKey>
    {

    }
    /// <summary>Интерфейс сущности тэга</summary>
    public interface ITag : ITag<int>, INamedEntity { }
}
