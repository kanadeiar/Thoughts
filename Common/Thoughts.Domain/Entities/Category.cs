using Thoughts.Domain.Base.Entities;

namespace Thoughts.Domain.Entities
{
    /// <summary>Сущность категории</summary>
    public class Category<TKey> : ICategory<TKey>
    {
        public TKey Id { get; set; } = default!;

        public string Name { get; set; } = null!;
    }

    /// <summary>Сущность категории</summary>
    public class Category : Category<int>, ICategory { }
}
