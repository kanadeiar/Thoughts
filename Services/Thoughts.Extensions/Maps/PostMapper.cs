using Thoughts.Interfaces.Base;

using PostDAL = Thoughts.DAL.Entities.Post;
using PostDOM = Thoughts.Domain.Base.Entities.Post;

using StatusDAL = Thoughts.DAL.Entities.Status;
using StatusDOM = Thoughts.Domain.Base.Entities.Status;

using UserDAL = Thoughts.DAL.Entities.User;
using UserDOM = Thoughts.Domain.Base.Entities.User;

using RoleDAL = Thoughts.DAL.Entities.Role;
using RoleDOM = Thoughts.Domain.Base.Entities.Role;

namespace Thoughts.Extensions.Maps;

public class PostMapper : IMapper<PostDOM, PostDAL>
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

    public PostDOM? Map(PostDAL? comment_dom)
    {
        if (comment_dom is null)
            return default;

        var post = new PostDOM
        {
            Id = comment_dom.Id,
            Status = ToDOM(comment_dom.Status),
            Date = comment_dom.Date,
            User = new UserDOM
            {
                Id = comment_dom.User.Id,
                LastName = comment_dom.User.LastName,
                FirstName = comment_dom.User.FirstName,
                Patronymic = comment_dom.User.Patronymic,
                Birthday = comment_dom.User.Birthday,
                NickName = comment_dom.User.NickName,
                Status = ToDOM(comment_dom.User.Status),
                Roles = comment_dom.User.Roles.Select(role => new RoleDOM
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToArray(),
            },
            Title = comment_dom.Title,
            Body = comment_dom.Body,
            PublicationsDate = comment_dom.PublicationDate,
            Category = (comment_dom.Category.Id, comment_dom.Category.Name),
            Comments = comment_dom.Comments.Select(comment => comment.Id).ToArray(),
            Tags = comment_dom.Tags.Select(tag => tag.Id).ToArray(),
        };

        return post;
    }

    public PostDAL? Map(PostDOM? comment_dom)
    {
        if (comment_dom is null) return default;

        var post = new PostDAL
        {
            Id = comment_dom.Id,
            Status = ToDAL(comment_dom.Status),
            Date = comment_dom.Date,
            Title = comment_dom.Title,
            Body = comment_dom.Body,
            PublicationDate = comment_dom.PublicationsDate,
            User = new UserDAL
            {
                Id = comment_dom.User.Id,
                LastName = comment_dom.User.LastName,
                FirstName = comment_dom.User.FirstName,
                Patronymic = comment_dom.User.Patronymic,
                Status = ToDAL(comment_dom.User.Status),
                Birthday = comment_dom.User.Birthday,
                NickName = comment_dom.User.NickName,
                Roles = comment_dom.User.Roles.Select(role => new RoleDAL { Id = role.Id, Name = role.Name }).ToArray(),
            }
        };

        return post;
    }
}