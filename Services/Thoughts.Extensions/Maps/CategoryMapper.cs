using Thoughts.DAL.Entities;
using Thoughts.Extensions.Base;

using CategoryDom = Thoughts.Domain.Base.Entities.Category;

namespace Thoughts.Extensions.Maps;

public class CategoryMapper : IMapper<CategoryDom, Category>, IMapper<Category, CategoryDom>
{
    public Category Map(CategoryDom item)
    {
        if (item is null) return default;
        var cat = new Category()
        {
            Id = item.Id,
            Name = item.Name,
            Status = (Status)item.Status,
        };
        MapsCash.CategoryDalCash.Add(cat);

        foreach (var post in item.Posts)
            cat.Posts.Add(MapsHelper.FindPostOrMapNew(post));

        return cat;
    }

    public CategoryDom Map(Category item)
    {
        if (item is null) return default;
        var cat = new Domain.Base.Entities.Category()
        {
            Id = item.Id,
            Name = item.Name,
            Status = (Domain.Base.Entities.Status)item.Status,
        };
        MapsCash.CategoryDomCash.Add(cat);

        foreach (var post in item.Posts)
            cat.Posts.Add(MapsHelper.FindPostOrMapNew(post));

        return cat;
    }
}