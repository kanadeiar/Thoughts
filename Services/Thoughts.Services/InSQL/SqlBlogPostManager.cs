using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.InSQL;

public class SqlBlogPostManager : IBlogPostManager
{
    private readonly ThoughtsDB db;
    private readonly ILogger logger;

    public SqlBlogPostManager(ThoughtsDB Db, ILogger Logger)
    {
        db = Db;
        logger = Logger;
    }

    #region Get all posts

    public async Task<IEnumerable<IPost>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        var posts = await db.Posts.ToArrayAsync(Cancel).ConfigureAwait(false);
        return (IEnumerable<IPost>)posts;
    }

    public async Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default)
    {
        var count = await db.Posts.CountAsync(Cancel).ConfigureAwait(false);
        return count;
    }

    public async Task<IEnumerable<IPost>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        if(Take == 0)
            return Enumerable.Empty<IPost>();

        var posts = await db.Posts
            .Skip(Skip)
            .Take(Take)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);

        return (IEnumerable<IPost>)posts;
    }

    public Task<IPage<IPost>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        //var totalCount = await db.Posts.CountAsync(Cancel).ConfigureAwait(false);

        //if (PageSize == 0)
        //    return new(Enumerable.Empty<IPost>(), PageIndex, PageSize, totalCount);

        //var posts = await db.Posts
        //    .Skip(PageIndex * PageSize)
        //    .Take(PageSize)
        //    .ToArrayAsync(Cancel)
        //    .ConfigureAwait(false);

        //return new(posts, PageIndex, PageSize, totalCount);

        throw new NotImplementedException();

    }

    #endregion

    #region Get user posts

    public Task<IEnumerable<IPost>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IPost>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<IPage<IPost>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    public Task<IPost?> GetPostAsync(int Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<IPost> CreatePostAsync(
        string Title,
        string Body,
        string UserId,
        string Category,
        CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    #region Tags

    public Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ITag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IPost>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Редактирование

    public Task<ICategory> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<IStatus> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }


    #endregion
}
