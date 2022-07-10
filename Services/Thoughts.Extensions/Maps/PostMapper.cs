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

    public PostDOM? Map(PostDAL? post_dom)
    {
        if (post_dom is null)
            return default;

        var post = new PostDOM
        {
            Id = post_dom.Id,
            Status = ToDOM(post_dom.Status),
            Date = post_dom.Date,
            User = new UserDOM
            {
                Id = post_dom.User.Id,
                LastName = post_dom.User.LastName,
                FirstName = post_dom.User.FirstName,
                Patronymic = post_dom.User.Patronymic,
                Birthday = post_dom.User.Birthday,
                NickName = post_dom.User.NickName,
                Status = ToDOM(post_dom.User.Status),
                Roles = post_dom.User.Roles.Select(role => new RoleDOM
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToArray(),
            },
            Title = post_dom.Title,
            Body = post_dom.Body,
            PublicationsDate = post_dom.PublicationDate,
            Category = (post_dom.Category.Id, post_dom.Category.Name),
            Comments = post_dom.Comments.Select(comment => comment.Id).ToArray(),
            Tags = post_dom.Tags.Select(tag => tag.Id).ToArray(),
        };

        return post;
    }

    public PostDAL? Map(PostDOM? post_dom)
    {
        if (post_dom is null) return default;

        var post = new PostDAL
        {
            Id = post_dom.Id,
            Status = ToDAL(post_dom.Status),
            Date = post_dom.Date,
            Title = post_dom.Title,
            Body = post_dom.Body,
            PublicationDate = post_dom.PublicationsDate,
            User = new UserDAL
            {
                Id = post_dom.User.Id,
                LastName = post_dom.User.LastName,
                FirstName = post_dom.User.FirstName,
                Patronymic = post_dom.User.Patronymic,
                Status = ToDAL(post_dom.User.Status),
                Birthday = post_dom.User.Birthday,
                NickName = post_dom.User.NickName,
                Roles = post_dom.User.Roles.Select(role => new RoleDAL { Id = role.Id, Name = role.Name }).ToArray(),
            }
        };

        return post;
    }
}