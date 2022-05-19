using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities
{
    /// <summary>
    /// Категория (раздел)
    /// </summary>
    [Index(nameof(Name),IsUnique =true, Name ="NameIndex")]
    public class Category:Entity
    {
        /// <summary>Наименование категории (раздела)</summary>      
        public string Name { get; set; } = null!;

        /// <summary>Список постов входящих в категорию</summary>
        public ICollection<Post> Posts { get; set; }=null!;

        public Category() { }

        public Category(string Name)=>this.Name = Name;

        public override string ToString() => Name;
    }
}
