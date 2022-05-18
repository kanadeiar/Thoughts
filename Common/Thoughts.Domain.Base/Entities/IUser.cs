using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    public interface IUser<TKey> : IPerson<TKey>, INamedEntity<TKey>
    {
        public int NotesCount { get; }
        public IEnumerable<INote> Notes { get; set; }
    }
    public interface IUser : IUser<int> { }
}
