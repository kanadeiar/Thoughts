
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность тэга</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class Tag<TKey> : ITag<TKey>
    {
        public TKey Id { get; set; } = default!;

        public string Name { get; set; } = null!;
    }

    /// <summary>Сущность тэга</summary>
    public class Tag : Tag<int>, ITag { }
}
