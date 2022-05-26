
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность пользователя, который может оставлять записи</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class User<TKey> : IUser<TKey>
    {
        public TKey Id { get; set; } = default!;

        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string Patronymic { get; set; } = null!;
    }

    /// <summary>Сущность пользователя, который может оставлять записи</summary>
    public class User : User<int>, IUser { }
}
