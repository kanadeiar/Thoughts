
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    public class Tag<TKey> : ITag<TKey>
    {
        public string Name { get; set; }
        public TKey Id { get; set; }
    }
    public class Tag : Tag<int>, ITag { }
}
