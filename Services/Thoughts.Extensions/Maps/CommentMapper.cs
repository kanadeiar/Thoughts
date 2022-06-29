using Thoughts.DAL.Entities;
using Thoughts.Interfaces.Base;

using CommentDom = Thoughts.Domain.Base.Entities.Comment;

namespace Thoughts.Extensions.Maps;

public class CommentMapper : IMapper<CommentDom, Comment>, IMapper<Comment, CommentDom>
{
    public Comment? Map(CommentDom? item)
    {
        if (item is null) return default;

        var com = new Comment
        {
            Id = item.Id,
            Body = item.Body,
            IsDeleted = item.IsDeleted,
            Date = item.Date,
        };
        MapsCash.CommentDalCash.Add(com);

        com.Post = MapsHelper.FindPostOrMapNew(item.Post);

        com.ParentComment = MapsHelper.FindCommentOrMapNew(item.ParentComment);

        com.User = MapsHelper.FindUserOrMapNew(item.User);

        foreach (var comment in item.ChildrenComment)
            com.ChildrenComment.Add(MapsHelper.FindCommentOrMapNew(comment));

        return com;
    }

    public CommentDom? Map(Comment? item)
    {
        if (item is null) return default;

        var com = new CommentDom
        {
            Id = item.Id,
            Body = item.Body,
            IsDeleted = item.IsDeleted,
            Date = item.Date,
        };
        MapsCash.CommentDomCash.Add(com);

        com.Post = MapsHelper.FindPostOrMapNew(item.Post);

        com.ParentComment = MapsHelper.FindCommentOrMapNew(item.ParentComment);

        com.User = MapsHelper.FindUserOrMapNew(item.User);

        foreach (var comment in item.ChildrenComment)
            com.ChildrenComment.Add(MapsHelper.FindCommentOrMapNew(comment));

        return com;
    }
}