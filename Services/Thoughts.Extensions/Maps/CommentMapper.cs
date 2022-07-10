using Thoughts.Interfaces.Base;

using CommentDAL = Thoughts.DAL.Entities.Comment;
using CommentDOM = Thoughts.Domain.Base.Entities.Comment;

using UserDAL = Thoughts.DAL.Entities.User;
using UserDOM = Thoughts.Domain.Base.Entities.User;

using RoleDAL = Thoughts.DAL.Entities.Role;
using RoleDOM = Thoughts.Domain.Base.Entities.Role;

using StatusDAL = Thoughts.DAL.Entities.Status;
using StatusDOM = Thoughts.Domain.Base.Entities.Status;

namespace Thoughts.Extensions.Maps;

public class CommentMapper : IMapper<CommentDOM, CommentDAL>
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

    public CommentDAL? Map(CommentDOM? comment_dom)
    {
        if (comment_dom is null) 
            return default;

        var comment_dal = new CommentDAL
        {
            Id = comment_dom.Id,
            Body = comment_dom.Body,
            IsDeleted = comment_dom.IsDeleted,
            Date = comment_dom.Date,
            ParentComment = Map(comment_dom.ParentComment),
            User = new UserDAL
            {
                Id = comment_dom.User.Id,
                Status = ToDAL(comment_dom.User.Status),
                LastName = comment_dom.User.LastName,
                FirstName = comment_dom.User.FirstName,
                Patronymic = comment_dom.User.Patronymic,
                Birthday = comment_dom.User.Birthday,
                NickName = comment_dom.User.NickName,
                Roles = comment_dom.User.Roles.Select(role => new RoleDAL { Id = role.Id, Name = role.Name }).ToArray(),
            }
        };

        comment_dal.ChildrenComment = comment_dom.ChildrenComment.Select(id => new CommentDAL
        {
            Id = id,
            ParentComment = comment_dal,
        }).ToArray();

        return comment_dal;
    }

    public CommentDOM? Map(CommentDAL? comment_dal)
    {
        if (comment_dal is null) 
            return default;

        var comment_dom = new CommentDOM
        {
            Id = comment_dal.Id,
            Body = comment_dal.Body,
            IsDeleted = comment_dal.IsDeleted,
            Date = comment_dal.Date,
            User = new UserDOM
            {
                Id = comment_dal.User.Id,
                Status = ToDOM(comment_dal.User.Status),
                LastName = comment_dal.User.LastName,
                FirstName = comment_dal.User.FirstName,
                Patronymic = comment_dal.User.Patronymic,
                NickName = comment_dal.User.NickName,
                Birthday = comment_dal.User.Birthday,
                Roles = comment_dal.User.Roles.Select(role => new RoleDOM { Id = role.Id, Name = role.Name, }).ToArray(),
            }
        };

        return comment_dom;
    }
}