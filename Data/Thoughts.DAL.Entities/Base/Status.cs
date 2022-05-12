
using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.Entities.Base
{
    [Index(nameof(Name), IsUnique = true, Name = "NameIndex")]
    public abstract class Status:Entity
    {
        /// <summary>Наименование статуса</summary>
        public string Name { get; set; }= null!;

        protected Status() { }

        protected Status(string name) => Name = name;
        public override string ToString() => Name;
    }
}
