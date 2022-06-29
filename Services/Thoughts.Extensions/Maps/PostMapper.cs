using Thoughts.Interfaces.Base;

using PostDal = Thoughts.DAL.Entities.Post;
using StatusDal = Thoughts.DAL.Entities.Status;
using PostDom = Thoughts.Domain.Base.Entities.Post;
using StatusDom = Thoughts.Domain.Base.Entities.Status;
using UserDom = Thoughts.Domain.Base.Entities.User;

namespace Thoughts.Extensions.Maps;

public class PostMapper : IMapper<PostDom, PostDal>, IMapper<PostDal, PostDom>
{
    public PostDom? Map(PostDal? item)
    {
        if (item is null) return default;

        var post = new PostDom
        {
            Id = item.Id,
            Status = (StatusDom)item.Status,
            Date = item.Date,
            User = new UserDom
            {
                Id = item.User.Id,
            },
            Title = item.Title,
            Body = item.Body,
            PublicationsDate = item.PublicationDate,
        };
        MapsCash.PostDomCash.Add(post);

        post.User = MapsHelper.FindUserOrMapNew(item.User);

        post.Category = MapsHelper.FindCategoryOrMapNew(item.Category);

        foreach (var tag in item.Tags)
            post.Tags.Add(MapsHelper.FindTagOrMapNew(tag));

        foreach (var comment in item.Comments)
            post.Comments.Add(MapsHelper.FindCommentOrMapNew(comment));

        return post;
    }

    public PostDal? Map(PostDom? item)
    {
        if(item is null) return default;

        var post = new PostDal
        {
            Id = item.Id,
            Status = (StatusDal)item.Status,
            Date = item.Date,
            Title = item.Title,
            Body = item.Body,
            PublicationDate = item.PublicationsDate,
        };
        MapsCash.PostDalCash.Add(post);

        post.User = MapsHelper.FindUserOrMapNew(item.User);

        post.Category = MapsHelper.FindCategoryOrMapNew(item.Category);

        foreach (var tag in item.Tags)
            post.Tags.Add(MapsHelper.FindTagOrMapNew(tag));

        foreach (var comment in item.Comments)
            post.Comments.Add(MapsHelper.FindCommentOrMapNew(comment));

        return post;
    }
}