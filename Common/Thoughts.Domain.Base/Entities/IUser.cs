using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    /// <summary>Интерфейс сущности пользователя, который может оставлять записи</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public interface IUser<TKey> : IPerson<TKey>, INamedEntity<TKey>
    {
        
    }

    /// <summary>Интерфейс сущности пользователя, который может оставлять записи</summary>
    public interface IUser : IUser<int>, IPerson, INamedEntity { }
}
