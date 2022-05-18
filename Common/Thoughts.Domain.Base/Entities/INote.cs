
using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    public interface INote<TKey> : IEntity<TKey>
    {
        public IUser<TKey> Creator { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
    public interface INote : INote<int>, IEntity { }
}
