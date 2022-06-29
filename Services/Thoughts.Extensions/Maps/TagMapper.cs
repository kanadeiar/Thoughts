using Thoughts.DAL.Entities;
using Thoughts.Extensions.Base;

using TagDom = Thoughts.Domain.Base.Entities.Tag;

namespace Thoughts.Extensions.Maps;

public class TagMapper : IMapper<TagDom, Tag>, IMapper<Tag, TagDom>
{
    public Tag Map(TagDom item)
    {
        if (item is null) return default;

        var tag = new Tag()
        {
            Id = item.Id,
            Name = item.Name,
        };
        MapsCash.TagDalCash.Add(tag);

        foreach (var post in item.Posts)
            tag.Posts.Add(MapsHelper.FindPostOrMapNew(post));

        return tag;
    }

    public TagDom Map(Tag item)
    {
        if (item is null) return default;

        var tag = new Domain.Base.Entities.Tag()
        {
            Id = item.Id,
            Name = item.Name,
        };
        MapsCash.TagDomCash.Add(tag);

        foreach (var post in item.Posts)
            tag.Posts.Add(MapsHelper.FindPostOrMapNew(post));

        return tag;
    }
}