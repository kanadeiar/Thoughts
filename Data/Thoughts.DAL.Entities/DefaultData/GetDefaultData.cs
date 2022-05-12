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
        public static Status[] DefaultStatus()
        {
            return new Status[]
            {
                new Status(){ Id=1, Name="Черновик"},
                new Status(){ Id=2, Name="Опубликовано"},
                new Status(){ Id=2, Name="Зблакировано"}
            };
        }

        public static Role[] DefaultRole()
        {
           return new Role[]
            {
              new Role(){ Id=1, Name="Администратор"},
              new Role(){ Id=1, Name="Модератор"},
              new Role(){ Id=1, Name="Автор"},
            };
         }
   }
}
