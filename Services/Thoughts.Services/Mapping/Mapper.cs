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
using FileDAL = Thoughts.DAL.Entities.File;

namespace Thoughts.Services.Mapping;

public static class Mapper
{
    #region To DAL
    public static PostDAL? PostToDAL(this PostDomain? postDomain)
        => postDomain is null ? null
        : new PostDAL
        {
            Id = postDomain.Id,
            Title = postDomain.Title,
            Body = postDomain.Body,
            User = postDomain.User.UserToDAL()!,
            Status = postDomain.Status.StatusToDAL()!,
            Category = postDomain.Category.CategoryToDAL()!,
            Tags = (ICollection<TagDAL>)postDomain.Tags.TagToDAL(),
            Comments = (ICollection<CommentDAL>)postDomain.Comments.CommentToDAL(),
            Files = (ICollection<FileDAL>)postDomain.Files.FileToDAL(),
            PublicationDate = postDomain.PublicationsDate,
            Date = postDomain.Date.DateTime,        // <- тут скорее всего плохо, всё же DateTimeOffset гораздо шире 
        };

    public static IEnumerable<PostDAL?> PostToDAL(this IEnumerable<PostDomain> postsDomain) => postsDomain.Select(PostToDAL);

    public static UserDAL? UserToDAL(this UserDomain? userDomain)
        => userDomain is null ? null
        : new UserDAL
        {
            Id = userDomain.Id,
            FirstName = userDomain.FirstName,
            LastName = userDomain.LastName,
            Patronymic = userDomain.Patronymic,
            NickName = userDomain.NickName,
            Birthday = userDomain.Birthday,
            Status = userDomain.Status.StatusToDAL()!,
            //Roles                 - нужно ли здесь?
        };


    public static StatusDAL? StatusToDAL(this StatusDomain? statusDomain)
        => statusDomain is null ? null
        : new StatusDAL
        {
            Id = statusDomain.Id,
            Name = statusDomain.Name,
            //Posts                 ??? - в DAL есть ссылка на посты, непонятно зачем и нужно ли здесь
        };


    public static CategoryDAL? CategoryToDAL(this CategoryDomain? categoryDomain)
        => categoryDomain is null ? null
        : new CategoryDAL
        {
            Id = categoryDomain.Id,
            Name = categoryDomain.Name,
            //Status = categoryDomain.Status.StatusToDAL(),     // <- непонятно зачем и нужно ли здесь
            //Posts                 ??? - в DAL есть ссылка на посты, непонятно зачем и нужно ли здесь
        };


    public static TagDAL? TagToDAL(this TagDomain? tagDomain)
        => tagDomain is null ? null
        : new TagDAL
        {
            Id = tagDomain.Id,
            Name = tagDomain.Name,
            //Posts                 ??? - в DAL есть ссылка на посты, непонятно зачем и нужно ли здесь
        };

    public static IEnumerable<TagDAL?> TagToDAL(this IEnumerable<TagDomain?> tagsDomain) => tagsDomain.Select(TagToDAL);


    public static CommentDAL? CommentToDAL(this CommentDomain? commentDomain)
        => commentDomain is null ? null
        : new CommentDAL
        {
            Id = commentDomain.Id,
            Body = commentDomain.Body,
            Date = commentDomain.Date,
            IsDeleted = commentDomain.IsDeleted,
            //ChildrenComment       <- не уверен что нужно здесь
            //ParentComment         <- не уверен что нужно здесь
            //Posts                 <- не уверен что нужно здесь
            //User                  <- не уверен что нужно здесь
        };

    public static IEnumerable<CommentDAL?> CommentToDAL(this IEnumerable<CommentDomain?> commentsDomain) => commentsDomain.Select(CommentToDAL);


    public static FileDAL? FileToDAL(this FileDomain? fileDomain)
        => fileDomain is null ? null
        : new FileDAL
        {
            Id = fileDomain.Id,
            Name = fileDomain.Name,
            FileBody = fileDomain.Content,
            FileDescription = fileDomain.Description,
            FileHash = fileDomain.Hash,
        };

    public static IEnumerable<FileDAL?> FileToDAL(this IEnumerable<FileDomain?> filesDomain) => filesDomain.Select(FileToDAL);

    #endregion

    #region To Domain
    public static PostDomain? PostToDomain(this PostDAL? postDAL)
    => postDAL is null ? null
    : new PostDomain
    {
        Id = postDAL.Id,
        Title = postDAL.Title,
        Body = postDAL.Body,
        User = postDAL.User.UserToDomain()!,
        Status = postDAL.Status.StatusToDomain()!,
        Category = postDAL.Category.CategoryToDomain()!,
        Tags = (ICollection<TagDomain>)postDAL.Tags.TagToDomain(),
        Comments = (ICollection<CommentDomain>)postDAL.Comments.CommentToDomain(),
        Files = (ICollection<FileDomain>)postDAL.Files.FileToDomain(),
        PublicationsDate = postDAL.PublicationDate,
        Date = postDAL.Date,         
    };

    public static IEnumerable<PostDomain?> PostToDomain(this IEnumerable<PostDAL?> postsDal) => postsDal.Select(PostToDomain);


    public static UserDomain? UserToDomain(this UserDAL? userDAL)
        => userDAL is null ? null
        : new UserDomain
        {
            Id = userDAL.Id,
            FirstName = userDAL.FirstName,
            LastName = userDAL.LastName,
            Patronymic = userDAL.Patronymic,
            NickName = userDAL.NickName,
            Birthday = userDAL.Birthday,
            Status = userDAL.Status.StatusToDomain()!,
        };


    public static StatusDomain? StatusToDomain(this StatusDAL? statusDAL)
        => statusDAL is null ? null
        : new StatusDomain
        {
            Id = statusDAL.Id,
            Name = statusDAL.Name,
        };


    public static CategoryDomain? CategoryToDomain(this CategoryDAL? categoryDAL)
        => categoryDAL is null ? null
        : new CategoryDomain
        {
            Id = categoryDAL.Id,
            Name = categoryDAL.Name,
        };


    public static TagDomain? TagToDomain(this TagDAL? tagDAL)
        => tagDAL is null ? null
        : new TagDomain
        {
            Id = tagDAL.Id,
            Name = tagDAL.Name,
        };

    public static IEnumerable<TagDomain?> TagToDomain(this IEnumerable<TagDAL?> tagsDAL) => tagsDAL.Select(TagToDomain);


    public static CommentDomain? CommentToDomain(this CommentDAL? commentDAL)
        => commentDAL is null ? null
        : new CommentDomain
        {
            Id = commentDAL.Id,
            Body = commentDAL.Body,
            Date = commentDAL.Date,
            IsDeleted = commentDAL.IsDeleted,
        };

    public static IEnumerable<CommentDomain?> CommentToDomain(this IEnumerable<CommentDAL?> commentsDAL) => commentsDAL.Select(CommentToDomain);


    public static FileDomain? FileToDomain(this FileDAL? fileDAL)
        => fileDAL is null ? null
        : new FileDomain
        {
            Id = fileDAL.Id,
            Name = fileDAL.Name,
            Content = fileDAL.FileBody,
            Description = fileDAL.FileDescription,
            Hash = fileDAL.FileHash,
        };

    public static IEnumerable<FileDomain?> FileToDomain(this IEnumerable<FileDAL?> filesDAL) => filesDAL.Select(FileToDomain);

    #endregion
}
