
using System.Linq;

using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    public class User<TKey> : IUser<TKey>
    {
        public int NotesCount => Notes.Count();
        public IEnumerable<INote> Notes { get; set; } = Enumerable.Empty<INote>();
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Name { get; set; }
        public TKey Id { get; set; }
    }
}
