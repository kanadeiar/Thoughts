using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.Entities.Base
{
    /// <summary>
    /// Категория (раздел)
    /// </summary>
    [Index(nameof(Name),IsUnique =true, Name ="NameIndex")]
    public abstract class Category:Entity
    {
        /// <summary>Наименование категории (раздела)</summary>      
        public string Name { get; set; } = null!;

        /// <summary>Список постов входящих в категорию</summary>
        public ICollection<Post> Posts { get; set; }=null!;

        protected Category() { }
        protected Category(string Name)=>this.Name = Name;
        public override string ToString() => Name;
    }
}
