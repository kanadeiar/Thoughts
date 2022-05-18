using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    public class Category<TKey> : ICategory<TKey>
    {
        public string Name { get; set; }
        public TKey Id { get; set; }
    }
    public class Category : Category<int>, ICategory { }
}
