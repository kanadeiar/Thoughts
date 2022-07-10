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

    public CategoryDAL? Map(CategoryDom? comment_dom)
    {
        if (comment_dom is null) 
            return default;

        var category_dal = new CategoryDAL
        {
            Id = comment_dom.Id,
            Name = comment_dom.Name,
            Status = ToDAL(comment_dom.Status),
            Posts = comment_dom.Posts.Select(id => new PostDAL { Id = id}).ToArray(),
        };

        return category_dal;
    }

    public CategoryDom? Map(CategoryDAL? comment_dom)
    {
        if (comment_dom is null) 
            return null;

        var cat = new CategoryDom
        {
            Id = comment_dom.Id,
            Name = comment_dom.Name,
            Status = ToDOM(comment_dom.Status),
            Posts = comment_dom.Posts.Select(p => p.Id).ToArray(),
        };

        return cat;
    }
}