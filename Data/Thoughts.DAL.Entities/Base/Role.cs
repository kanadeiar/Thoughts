using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.Entities.Base
{
    [Index(nameof(Name), IsUnique =true, Name ="NameIndex")]
    public abstract class Role:Entity
    {
        public string Name { get; set; } = null!;

        protected Role() { }

        protected Role(string name)=>Name = name;

        public override string ToString() => Name;
    }
}
