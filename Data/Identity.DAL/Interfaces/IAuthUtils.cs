using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DAL.Interfaces
{
    public interface IAuthUtils<TUser>
    {
        string CreateSessionToken(TUser user, IList<string> role);
    }
}
