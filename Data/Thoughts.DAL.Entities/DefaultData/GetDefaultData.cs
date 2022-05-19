using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities.DefaultData
{
    public static class GetDefaultData
    {
        public static Status[] DefaultStatus() => new Status[]
            {
                new (){ Id=1, Name="Черновик"},
                new (){ Id=2, Name="Опубликовано"},
                new (){ Id=3, Name="Зблакировано"}
            };

        public static Role[] DefaultRole() => new Role[]
             {
              new (){ Id=1, Name="Администратор"},
              new (){ Id=2, Name="Модератор"},
              new (){ Id=3, Name="Автор"},
             };
    }
}
