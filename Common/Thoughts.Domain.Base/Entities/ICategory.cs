
using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    /// <summary>Интерфейс сущности категории</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public interface ICategory<TKey> : INamedEntity<TKey>
    {

    }
    /// <summary>Интерфейс сущности категории</summary>
    public interface ICategory : ICategory<int>, INamedEntity { }
}
