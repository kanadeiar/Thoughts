
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность комментария</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class Comment<TKey> : IComment<TKey>
    {
        public TKey Id { get; set; } = default!;

        public string Text { get; set; } = null!;

        public IUser<TKey> Creator { get; set; } = default!;

        public DateTime CreationDate { get; set; }

        public DateTime? EditDate { get; set; }

        public ICollection<IComment<TKey>> Answers { get; set; } = new HashSet<IComment<TKey>>();
    }

    /// <summary>Сущность комментария</summary>
    public class Comment : Comment<int>, IComment { }
}
