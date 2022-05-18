
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность комментария</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class Comment<TKey> : IComment<TKey>
    {
        public IEnumerable<IComment<TKey>> Answers { get; set; } = Enumerable.Empty<IComment<TKey>>();
        public IUser<TKey> Creator { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }
        public TKey Id { get; set; }
    }
    /// <summary>Сущность комментария</summary>
    public class Comment : Comment<int>, IComment { }
}
