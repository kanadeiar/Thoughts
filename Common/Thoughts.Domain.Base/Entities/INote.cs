
using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    public interface INote<TKey> : IEntity<TKey>
    {
        public IUser User { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
    public interface INote : INote<int> { }
}
