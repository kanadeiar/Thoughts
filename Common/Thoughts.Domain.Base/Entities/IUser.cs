using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    /// <summary>Интерфейс сущности пользователя, который может оставлять записи</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public interface IUser<TKey> : IPerson<TKey>, INamedEntity<TKey>
    {
        /// <summary>Количество записей пользователя</summary>
        public int BlogNotesCount { get; }

        /// <summary>Количество коментариев пользователя</summary>
        public int CommentsCount { get; }

        /// <summary>Перечисление записей пользователя</summary>
        /// /// <typeparam name="TKey">Тип первичного ключа</typeparam>
        public IEnumerable<INote<TKey>> BlogNotes { get; set; }

        /// <summary>Перечисление комментариев пользователя</summary>
        /// /// <typeparam name="TKey">Тип первичного ключа</typeparam>
        public IEnumerable<INote<TKey>> Comments { get; set; }
    }
    /// <summary>Интерфейс сущности пользователя, который может оставлять записи</summary>
    public interface IUser : IUser<int>, IPerson, INamedEntity { }
}
