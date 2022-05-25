using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность категории</summary>
    public class Category<TKey> : ICategory<TKey>
    {
        public string Name { get; set; }
        public TKey Id { get; set; }
    }
    /// <summary>Сущность категории</summary>
    public class Category : Category<int>, ICategory { }
}
