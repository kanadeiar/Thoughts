namespace Thoughts.Domain.Base.Entities
{
    ///<summary>Интерфейс сущности записи блога</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public interface IBlogNote<TKey> : INote<TKey>
    {
        ///<summary>Заголовок записи блога</summary>
        public string Title { get; set; }

        ///<summary>Перечисление категорий/разделов записи блога</summary>
        /// <typeparam name="TKey">Тип первичного ключа</typeparam>
        public ICategory<TKey> Category { get; set; }

        ///<summary>Перечисление тэгов записи блога</summary>
        /// <typeparam name="TKey">Тип первичного ключа</typeparam>
        public ICollection<ITag<TKey>> Tags { get; set; }

        ///<summary>Перечисление комментариев записи блога</summary>
        /// <typeparam name="TKey">Тип первичного ключа</typeparam>
        public ICollection<IComment<TKey>> Comments { get; set; }
    }

    ///<summary>Интерфейс сущности записи блога</summary>
    public interface IBlogNote : IBlogNote<int>, INote { }
}
