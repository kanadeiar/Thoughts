using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thoughts.Interfaces.Base.Entities
{
    internal interface IUser<TKye>:IEntity<TKye>
    {
        /// <summary>Фамилия</summary>
        public string LastName { get; set; }

        /// <summary>Имя</summary>
        public string FirstName { get; set; }

        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }

        /// <summary>Дата рождения</summary>
        public DateTime Birthday { get; set; }

        /// <summary>Псевдоним (отображаемое имя автора)</summary>
        public string NikName { get; set; }
    }
}
