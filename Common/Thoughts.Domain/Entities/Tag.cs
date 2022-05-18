
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность тэга</summary>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class Tag<TKey> : ITag<TKey>
    {
        public string Name { get; set; }
        public TKey Id { get; set; }
    }
    /// <summary>Сущность тэга</summary>
    public class Tag : Tag<int>, ITag { }
}
