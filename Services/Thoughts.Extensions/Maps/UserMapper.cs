using Thoughts.DAL.Entities;
using Thoughts.Interfaces.Base;

using UserDom = Thoughts.Domain.Base.Entities.User;

namespace Thoughts.Extensions.Maps;

public class UserMapper : IMapper<UserDom, User>, IMapper<User, UserDom>
{
    public User? Map(UserDom? item)
    {
        if (item is null) return default;

        var user = new User
        {
            Id = item.Id,
            NickName = item.NickName,
            LastName = item.LastName,
            FirstName = item.FirstName,
            Patronymic = item.Patronymic,
            Birthday = item.Birthday,
            Status = (Status)item.Status,
        };
        MapsCash.UserDalCash.Add(user);

        foreach (var role in item.Roles)
            user.Roles.Add(MapsHelper.FindRoleOrMapNew(role));

        return user;
    }

    public UserDom? Map(User? item)
    {
        if (item is null) return default;

        var user = new UserDom
        {
            Id = item.Id,
            NickName = item.NickName,
            LastName = item.LastName,
            FirstName = item.FirstName,
            Patronymic = item.Patronymic,
            Birthday = item.Birthday,
            Status = (Domain.Base.Entities.Status)item.Status,
        };
        MapsCash.UserDomCash.Add(user);

        foreach (var role in item.Roles)
            user.Roles.Add(MapsHelper.FindRoleOrMapNew(role));

        return user;
    }
}