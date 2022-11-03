using System.ComponentModel;

using Thoughts.DAL.Entities;

using PostDomain = Thoughts.Domain.Base.Entities.Post;
using PostDAL = Thoughts.DAL.Entities.Post;

using UserDomain = Thoughts.Domain.Base.Entities.User;
using UserDAL = Thoughts.DAL.Entities.User;

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
    public static PostDAL? PostToDAL(this PostDomain? post)
        => post is null ? null
        : new PostDAL
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            User = post.User.UserToDAL()!,
            Status = post.Status.StatusToDAL(),
            Category = post.Category.CategoryToDAL()!,
            Tags = post.Tags.TagToDAL().ToList(),
            Comments = post.Comments.CommentToDAL().ToList(),
            PublicationDate = post.PublicationsDate,
            Date = post.Date
        };

    public static IEnumerable<PostDAL?> PostToDAL(this IEnumerable<PostDomain> posts) => posts.Select(PostToDAL);

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
            Status = user.Status.StatusToDAL()!,
            //Roles                 - нужно ли здесь?
        };


    public static StatusDAL StatusToDAL(this StatusDomain status) => status switch
    {
        StatusDomain.Private => Status.Private,
        StatusDomain.Protected => Status.Protected,
        StatusDomain.Public => Status.Public,
        StatusDomain.Blocked => Status.Blocked,
        _ => throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(StatusDomain))
    };


    public static CategoryDAL? CategoryToDAL(this CategoryDomain? category)
        => category is null ? null
        : new CategoryDAL
        {
            Id = category.Id,
            Name = category.Name,
            //Status = categoryDomain.Status.StatusToDAL(),     // <- непонятно зачем и нужно ли здесь
            //Posts                 ??? - в DAL есть ссылка на посты, непонятно зачем и нужно ли здесь
        };


    public static TagDAL? TagToDAL(this TagDomain? tag)
        => tag is null ? null
        : new TagDAL
        {
            Id = tag.Id,
            Name = tag.Name,
            //Posts                 ??? - в DAL есть ссылка на посты, непонятно зачем и нужно ли здесь
        };

    public static IEnumerable<TagDAL?> TagToDAL(this IEnumerable<TagDomain?> tagsDomain) => tagsDomain.Select(TagToDAL);


    public static CommentDAL? CommentToDAL(this CommentDomain? comment)
        => comment is null ? null
        : new CommentDAL
        {
            Id = comment.Id,
            Body = comment.Body,
            Date = comment.Date,
            IsDeleted = comment.IsDeleted,
            //ChildrenComment       <- не уверен что нужно здесь
            //ParentComment         <- не уверен что нужно здесь
            //Posts                 <- не уверен что нужно здесь
            //User                  <- не уверен что нужно здесь
        };

    public static IEnumerable<CommentDAL?> CommentToDAL(this IEnumerable<CommentDomain?> comments) => comments.Select(CommentToDAL);


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

    public static IEnumerable<ContentFile?> FileToDAL(this IEnumerable<FileDomain?> files) => files.Select(FileToDAL);

    #endregion

    #region To Domain
    public static PostDomain? PostToDomain(this PostDAL? post)
    => post is null ? null
    : new PostDomain
    {
        Id = post.Id,
        Title = post.Title,
        Body = post.Body,
        User = post.User.UserToDomain()!,
        Status = post.Status.StatusToDomain()!,
        Category = post.Category.CategoryToDomain()!,
        Tags = post.Tags.TagToDomain().ToArray()!,
        Comments = post.Comments.CommentToDomain().ToArray()!,
        PublicationsDate = post.PublicationDate,
        Date = post.Date,
    };

    public static IEnumerable<PostDomain?> PostToDomain(this IEnumerable<PostDAL?> posts) => posts.Select(PostToDomain);


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
        Status.Private => StatusDomain.Private,
        Status.Protected => StatusDomain.Protected,
        Status.Public => StatusDomain.Public,
        Status.Blocked => StatusDomain.Blocked,
        _ => throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(StatusDAL))
    };


    public static CategoryDomain? CategoryToDomain(this CategoryDAL? category)
        => category is null ? null
        : new CategoryDomain
        {
            Id = category.Id,
            Name = category.Name,
        };


    public static TagDomain? TagToDomain(this TagDAL? tag)
        => tag is null ? null
        : new TagDomain
        {
            Id = tag.Id,
            Name = tag.Name,
        };

    public static IEnumerable<TagDomain?> TagToDomain(this IEnumerable<TagDAL?> tags) => tags.Select(TagToDomain);


    public static CommentDomain? CommentToDomain(this CommentDAL? comment)
        => comment is null ? null
        : new CommentDomain
        {
            Id = comment.Id,
            Body = comment.Body,
            Date = comment.Date,
            IsDeleted = comment.IsDeleted,
        };

    public static IEnumerable<CommentDomain?> CommentToDomain(this IEnumerable<CommentDAL?> comments) => comments.Select(CommentToDomain);


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

    public static IEnumerable<FileDomain?> FileToDomain(this IEnumerable<ContentFile?> files) => files.Select(FileToDomain);

    #endregion
}
