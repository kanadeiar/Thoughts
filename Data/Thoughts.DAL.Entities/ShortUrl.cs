using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities
{
    public  class ShortUrl:Entity
    {
        /// <summary> Оригинальный URL </summary>
        [Required]
        public Uri OriginalUrl { get; set; }

        /// <summary> Псевдоним ссылки </summary>
        [Required]
        public string Alias { get; set; }
    }
}
