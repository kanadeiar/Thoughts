using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.Entities.Base
{
    [Index(nameof(Name), IsUnique = true, Name = "NameIndex")]
    public class Role : Entity
    {
        public string Name { get; set; } = null!;

        public ICollection<User> Users { get; set; } = null!;
        public Role() { }

        public Role(string name) => Name = name;

        public override string ToString() => Name;

    }
}
