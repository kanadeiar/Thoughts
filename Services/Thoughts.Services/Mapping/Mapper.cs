using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Thoughts.DAL.Entities;
using Thoughts.Domain.Base.Entities;

using PostDomain = Thoughts.Domain.Base.Entities.Post;
using PostDAL = Thoughts.DAL.Entities.Post;

using UserDomain = Thoughts.Domain.Base.Entities.User;
using UserDAL = Thoughts.DAL.Entities.User;

using RoleDomain = Thoughts.Domain.Base.Entities.Role;
using RoleDAL = Thoughts.DAL.Entities.Role;

using StatusDomain = Thoughts.Domain.Base.Entities.Status;
using StatusDAL = Thoughts.DAL.Entities.Status;

using CategoryDomain = Thoughts.Domain.Base.Entities.Category;
using CategoryDAL = Thoughts.DAL.Entities.Category;

using TagDomain = Thoughts.Domain.Base.Entities.Tag;
using TagDAL = Thoughts.DAL.Entities.Tag;

using CommentDomain = Thoughts.Domain.Base.Entities.Comment;
using CommentDAL = Thoughts.DAL.Entities.Comment;

using FileDomain = Thoughts.Domain.Base.Entities.FileModel;

namespace Thoughts.Services.Mapping;

public static class Mapper
{
    #region To DAL

    [return: NotNullIfNotNull("post")]
    public static PostDAL? PostToDAL(this PostDomain? post)
        => post is null ? null
        : new PostDAL
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            User = post.User.UserToDAL()!,
            Status = post.Status.StatusToDAL(),
            Category = new() { Id = post.Category.Id, Name = post.Category.Name },
            Tags = post.Tags.Select(id => new TagDAL { Id = id }).ToArray(),
            Comments = post.Comments.Select(id => new CommentDAL { Id = id }).ToArray(),
            PublicationDate = post.PublicationsDate,
            Date = post.Date
        };

    public static IEnumerable<PostDAL> PostToDAL(this IEnumerable<PostDomain> posts) => posts.Select(post => PostToDAL(post));

    [return: NotNullIfNotNull("user")]
    public static UserDAL? UserToDAL(this UserDomain? user)
        => user is null ? null
        : new UserDAL
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            NickName = user.NickName,
            Birthday = user.Birthday,
            Status = user.Status.StatusToDAL(),
            Roles = user.Roles.Select(role => new RoleDAL { Id = role.Id, Name = role.Name }).ToArray(),
        };


    public static StatusDAL StatusToDAL(this StatusDomain status) => status switch
    {
        StatusDomain.Private => StatusDAL.Private,
        StatusDomain.Protected => StatusDAL.Protected,
        StatusDomain.Public => StatusDAL.Public,
        StatusDomain.Blocked => StatusDAL.Blocked,
        _ => throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(StatusDomain))
    };


    [return: NotNullIfNotNull("category")]
    public static CategoryDAL? CategoryToDAL(this CategoryDomain? category)
        => category is null ? null
        : new CategoryDAL
        {
            Id = category.Id,
            Name = category.Name,
            Status = category.Status.StatusToDAL(),
            Posts = category.Posts.Select(id => new PostDAL { Id = id }).ToArray(),
        };

    [return: NotNullIfNotNull("tag")]
    public static TagDAL? TagToDAL(this TagDomain? tag)
        => tag is null ? null
        : new TagDAL
        {
            Id = tag.Id,
            Name = tag.Name,
            Posts = tag.Posts.Select(id => new PostDAL { Id = id }).ToArray(),
        };

    public static IEnumerable<TagDAL> TagToDAL(this IEnumerable<TagDomain> Tags) => Tags.Select(tag => TagToDAL(tag));

    [return: NotNullIfNotNull("comment")]
    public static CommentDAL? CommentToDAL(this CommentDomain? comment)
        => comment is null ? null
        : new CommentDAL
        {
            Id = comment.Id,
            Body = comment.Body,
            Date = comment.Date,
            IsDeleted = comment.IsDeleted,
            User = comment.User.UserToDAL(),
            Post = new PostDAL { Id = comment.PostId },
            ParentComment = comment.ParentComment.CommentToDAL(),
            ChildrenComment = comment.ChildrenComment.Select(id => new CommentDAL { Id = id}).ToArray(),
        };

    public static IEnumerable<CommentDAL> CommentToDAL(this IEnumerable<CommentDomain> comments) => comments.Select(comment => CommentToDAL(comment));

    [return: NotNullIfNotNull("file")]
    public static ContentFile? FileToDAL(this FileDomain? file)
        => file is null ? null
        : new ContentFile
        {
            Id = file.Id,
            Name = file.Name,
            FileBody = file.Content,
            FileDescription = file.Description,
            FileHash = file.Hash,
        };

    public static IEnumerable<ContentFile> FileToDAL(this IEnumerable<FileDomain> files) => files.Select(file => FileToDAL(file));

    #endregion

    #region To Domain

    [return: NotNullIfNotNull("post")]
    public static PostDomain? PostToDomain(this PostDAL? post)
    => post is null ? null
    : new PostDomain
    {
        Id = post.Id,
        Title = post.Title,
        Body = post.Body,
        User = post.User.UserToDomain()!,
        Status = post.Status.StatusToDomain()!,
        Category = (post.Category.Id, post.Category.Name),
        Tags = post.Tags.Select(tag => tag.Id).ToArray(),
        Comments = post.Comments.Select(comment => comment.Id).ToArray(),
        PublicationsDate = post.PublicationDate,
        Date = post.Date,
    };

    public static IEnumerable<PostDomain> PostToDomain(this IEnumerable<PostDAL> posts) => posts.Select(post => PostToDomain(post));

    [return: NotNullIfNotNull("user")]
    public static UserDomain? UserToDomain(this UserDAL? user)
        => user is null ? null
        : new UserDomain
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            NickName = user.NickName,
            Birthday = user.Birthday,
            Status = user.Status.StatusToDomain(),
        };


    public static StatusDomain StatusToDomain(this StatusDAL status) => status switch
    {
        StatusDAL.Private => StatusDomain.Private,
        StatusDAL.Protected => StatusDomain.Protected,
        StatusDAL.Public => StatusDomain.Public,
        StatusDAL.Blocked => StatusDomain.Blocked,
        _ => throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(StatusDAL))
    };

    [return: NotNullIfNotNull("category")]
    public static CategoryDomain? CategoryToDomain(this CategoryDAL? category)
        => category is null ? null
        : new CategoryDomain
        {
            Id = category.Id,
            Name = category.Name,
        };


    [return: NotNullIfNotNull("tag")]
    public static TagDomain? TagToDomain(this TagDAL? tag)
        => tag is null ? null
        : new TagDomain
        {
            Id = tag.Id,
            Name = tag.Name,
        };

    public static IEnumerable<TagDomain> TagToDomain(this IEnumerable<TagDAL> tags) => tags.Select(tag => TagToDomain(tag));

    [return: NotNullIfNotNull("comment")]
    public static CommentDomain? CommentToDomain(this CommentDAL? comment)
        => comment is null ? null
        : new CommentDomain
        {
            Id = comment.Id,
            Body = comment.Body,
            Date = comment.Date,
            IsDeleted = comment.IsDeleted,
        };

    public static IEnumerable<CommentDomain> CommentToDomain(this IEnumerable<CommentDAL> comments) => comments.Select(comment => CommentToDomain(comment));

    [return: NotNullIfNotNull("file")]
    public static FileDomain? FileToDomain(this ContentFile? file)
        => file is null ? null
        : new FileDomain
        {
            Id = file.Id,
            Name = file.Name,
            Content = file.FileBody,
            Description = file.FileDescription,
            Hash = file.FileHash,
        };

    public static IEnumerable<FileDomain> FileToDomain(this IEnumerable<ContentFile> files) => files.Select(file => FileToDomain(file));

    #endregion
}
