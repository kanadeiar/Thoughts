namespace Thoughts.Domain.Base.Entities
{
    /// <summary>Интерфейс сущности комментария</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public interface IComment<TKey> : INote<TKey>
    {
        /// <summary>Перечисление ответов на коментарии</summary>
        /// <typeparam name="TKey">Тип первичного ключа</typeparam>
        public ICollection<IComment<TKey>> Answers { get; set; }
    }
    /// <summary>Интерфейс сущности комментария</summary>
    public interface IComment : IComment<int>, INote { }
}
