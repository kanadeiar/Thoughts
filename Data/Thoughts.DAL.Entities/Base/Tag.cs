using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.Entities.Base
{
    /// <summary>
    /// Тег
    /// </summary>
    [Index(nameof(Name), IsUnique =true, Name = "NameIndex")]
    public abstract class Tag:Entity
    {
        /// <summary>Название тега</summary>
        public string Name { get; set; } = null!;

        protected Tag() { }

        protected Tag(string Name)=>this.Name = Name;

        public override string ToString() => Name;
    }
}
