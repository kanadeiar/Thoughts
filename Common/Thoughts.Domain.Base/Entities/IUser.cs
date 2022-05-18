using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities
{
    public interface IUser : IPerson, INamedEntity
    {
        public string Nickname { get; set; }
        public int NotesCount { get; set; }
    }
}
