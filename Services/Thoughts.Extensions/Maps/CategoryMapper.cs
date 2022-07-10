using Thoughts.DAL.Entities;
using Thoughts.Interfaces.Base;

using PostDAL = Thoughts.DAL.Entities.Post;
using CategoryDom = Thoughts.Domain.Base.Entities.Category;
using CategoryDAL = Thoughts.DAL.Entities.Category;

namespace Thoughts.Extensions.Maps;

public class CategoryMapper : IMapper<CategoryDom, CategoryDAL>
{
    private static Domain.Base.Entities.Status ToDom(Status status_dal) => status_dal switch
    {
        Status.Blocked => Domain.Base.Entities.Status.Blocked,
        Status.Private => Domain.Base.Entities.Status.Private,
        Status.Protected => Domain.Base.Entities.Status.Protected,
        Status.Public => Domain.Base.Entities.Status.Public,
        _ => (Domain.Base.Entities.Status)(int)status_dal
    };

    private static Status ToDAL(Domain.Base.Entities.Status status_dal) => status_dal switch
    {
        Domain.Base.Entities.Status.Blocked => Status.Blocked,
        Domain.Base.Entities.Status.Private => Status.Private,
        Domain.Base.Entities.Status.Protected => Status.Protected,
        Domain.Base.Entities.Status.Public => Status.Public,
        _ => (Status)(int)status_dal
    };

    public CategoryDAL? Map(CategoryDom? category_dom)
    {
        if (category_dom is null) 
            return default;

        var category_dal = new CategoryDAL
        {
            Id = category_dom.Id,
            Name = category_dom.Name,
            Status = ToDAL(category_dom.Status),
            Posts = category_dom.Posts.Select(id => new PostDAL { Id = id}).ToArray(),
        };

        return category_dal;
    }

    public CategoryDom? Map(CategoryDAL? category_dal)
    {
        if (category_dal is null) 
            return null;

        var cat = new CategoryDom
        {
            Id = category_dal.Id,
            Name = category_dal.Name,
            Status = ToDom(category_dal.Status),
            Posts = category_dal.Posts.Select(p => p.Id).ToArray(),
        };

        return cat;
    }
}