using Thoughts.Interfaces.Base;

using TagDAL = Thoughts.DAL.Entities.Tag;
using TagDOM = Thoughts.Domain.Base.Entities.Tag;

using PostDAL = Thoughts.DAL.Entities.Post;

namespace Thoughts.Extensions.Maps;

public class TagMapper : IMapper<TagDOM, TagDAL>
{
    public TagDAL? Map(TagDOM? tag_dom)
    {
        if (tag_dom is null) 
            return default;

        var tag_dal = new TagDAL
        {
            Id = tag_dom.Id,
            Name = tag_dom.Name,
            Posts = tag_dom.Posts.Select(id => new PostDAL { Id = id }).ToArray(),
        };

        return tag_dal;
    }

    public TagDOM? Map(TagDAL? tag_dal)
    {
        if (tag_dal is null) 
            return default;

        var tag_dom = new TagDOM
        {
            Id = tag_dal.Id,
            Name = tag_dal.Name,
            Posts = tag_dal.Posts.Select(post => post.Id).ToArray(),
        };

        return tag_dom;
    }
}