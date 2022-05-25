
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность пользователя, который может оставлять записи</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class User<TKey> : IUser<TKey>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Name { get; set; }
        public TKey Id { get; set; }
        public int BlogNotesCount => BlogNotes.Count();
        public int CommentsCount => Comments.Count();
        public IEnumerable<INote<TKey>> BlogNotes { get; set; } = Enumerable.Empty<INote<TKey>>();
        public IEnumerable<INote<TKey>> Comments { get; set; } = Enumerable.Empty<INote<TKey>>();
    }
    /// <summary>Сущность пользователя, который может оставлять записи</summary>
    public class User : User<int>, IUser { }
}
