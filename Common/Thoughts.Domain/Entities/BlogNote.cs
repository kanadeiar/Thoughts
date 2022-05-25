using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    ///<summary>Сущность записи блога</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class BlogNote<TKey> : IBlogNote<TKey>
    {
        public string Title { get; set; }
        public ICategory<TKey> Category { get; set; }
        public IEnumerable<ITag<TKey>> Tags { get; set; }
        public IEnumerable<IComment<TKey>> Comments { get; set; }
        public IUser<TKey> Creator { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }
        public TKey Id { get; set; }
    }
    ///<summary>Сущность записи блога</summary>
    public class BlogNote : BlogNote<int>, IBlogNote { }
}
