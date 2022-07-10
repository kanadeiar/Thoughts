using Thoughts.Interfaces.Base;

using PostDAL = Thoughts.DAL.Entities.Post;
using CategoryDom = Thoughts.Domain.Base.Entities.Category;
using CategoryDAL = Thoughts.DAL.Entities.Category;

using StatusDAL = Thoughts.DAL.Entities.Status;
using StatusDOM = Thoughts.Domain.Base.Entities.Status;

namespace Thoughts.Extensions.Maps;

public class CategoryMapper : IMapper<CategoryDom, CategoryDAL>
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

    public CategoryDAL? Map(CategoryDom? post_dom)
    {
        if (post_dom is null) 
            return default;

        var category_dal = new CategoryDAL
        {
            Id = post_dom.Id,
            Name = post_dom.Name,
            Status = ToDAL(post_dom.Status),
            Posts = post_dom.Posts.Select(id => new PostDAL { Id = id}).ToArray(),
        };

        return category_dal;
    }

    public CategoryDom? Map(CategoryDAL? post_dom)
    {
        if (post_dom is null) 
            return null;

        var cat = new CategoryDom
        {
            Id = post_dom.Id,
            Name = post_dom.Name,
            Status = ToDOM(post_dom.Status),
            Posts = post_dom.Posts.Select(p => p.Id).ToArray(),
        };

        return cat;
    }
}