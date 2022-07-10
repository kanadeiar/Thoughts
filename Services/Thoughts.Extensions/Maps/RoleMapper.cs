using Thoughts.DAL.Entities;
using Thoughts.Interfaces.Base;

using RoleDAL = Thoughts.DAL.Entities.Role;
using RoleDOM = Thoughts.Domain.Base.Entities.Role;

using StatusDAL = Thoughts.DAL.Entities.Status;
using StatusDOM = Thoughts.Domain.Base.Entities.Status;

namespace Thoughts.Extensions.Maps;

public class RoleMapper : IMapper<RoleDOM, Role>
{
    private static StatusDOM ToDOM(StatusDAL status_dal) => status_dal switch
    {
        StatusDAL.Blocked => StatusDOM.Blocked,
        StatusDAL.Private => StatusDOM.Private,
        StatusDAL.Protected => StatusDOM.Protected,
        StatusDAL.Public => StatusDOM.Public,
        _ => (StatusDOM)(int)status_dal
    };

    private static StatusDAL ToDAL(StatusDOM status_dal) => status_dal switch
    {
        StatusDOM.Blocked => StatusDAL.Blocked,
        StatusDOM.Private => StatusDAL.Private,
        StatusDOM.Protected => StatusDAL.Protected,
        StatusDOM.Public => StatusDAL.Public,
        _ => (StatusDAL)(int)status_dal
    };

    public RoleDOM? Map(RoleDAL? role_dal)
    {
        if (role_dal is null) 
            return default;

        var role_dom = new RoleDOM
        {
            Id = role_dal.Id,
            Name = role_dal.Name,
        };

        return role_dom;
    }

    public RoleDAL? Map(RoleDOM? role_dom)
    {
        if (role_dom is null) return default;

        var role = new Role
        {
            Id = role_dom.Id,
            Name = role_dom.Name,
        };

        return role;
    }
}