
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    public class Comment<TKey> : IComment<TKey>
    {
        public IEnumerable<IComment<TKey>> Answers { get; set; }
        public IUser<TKey> Creator { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }
        public TKey Id { get; set; }
    }
    public class Comment : Comment<int>, IComment { }
}
