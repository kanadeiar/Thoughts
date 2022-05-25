
using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    /// <summary>Интерфейс сущности записи</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public interface INote<TKey> : IEntity<TKey>
    {
        /// <summary>Автор записи</summary>
        /// <typeparam name="TKey">Тип первичного ключа</typeparam>
        public IUser<TKey> Creator { get; set; }

        /// <summary>Текст записи</summary>
        public string Text { get; set; }

        /// <summary>Дата создания</summary>
        public DateTime CreationDate { get; set; }

        /// <summary>Время редактирования</summary>
        public DateTime? EditDate { get; set; }
    }

    /// <summary>Интерфейс сущности записи</summary>
    public interface INote : INote<int>, IEntity { }
}
