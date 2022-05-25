using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    ///<summary>Сущность записи блога</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class BlogNote<TKey> : IBlogNote<TKey>
    {
        public TKey Id { get; set; } = default!;

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime? EditDate { get; set; }

        public IUser<TKey> Creator { get; set; } = null!;

        public ICategory<TKey> Category { get; set; } = null!;

        public ICollection<ITag<TKey>> Tags { get; set; } = new HashSet<ITag<TKey>>();

        public ICollection<IComment<TKey>> Comments { get; set; } = new HashSet<IComment<TKey>>();
    }

    ///<summary>Сущность записи блога</summary>
    public class BlogNote : BlogNote<int>, IBlogNote { }
}
