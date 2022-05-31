using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Domain;
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

    public async Task<IPage<IPost>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var totalCount = await db.Posts.CountAsync(Cancel).ConfigureAwait(false);

        if (PageSize == 0)
            return new Page<IPost>(Enumerable.Empty<IPost>(), PageIndex, PageSize, totalCount);

        var posts = await db.Posts
            .Skip(PageIndex * PageSize)
            .Take(PageSize)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);

        return new Page<IPost>((IEnumerable<IPost>)posts,PageIndex,PageSize,totalCount);
    }

    #endregion

    #region Get user posts

    public async Task<IEnumerable<IPost>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        var userPosts = await db.Posts
            .FirstOrDefaultAsync(p => p.UserId == int.Parse(UserId), Cancel)
            .ConfigureAwait(false);

        return (IEnumerable<IPost>)userPosts!;
    }

    public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        int count = await db.Posts
            .CountAsync(p => p.UserId == int.Parse(UserId), Cancel)
            .ConfigureAwait(false);
            
        return count;
    }

    public async Task<IEnumerable<IPost>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
    {
        if (Take == 0)
            return Enumerable.Empty<IPost>();

        var posts = await GetAllPostsByUserIdAsync(UserId, Cancel);
        var page = posts.Skip(Skip).Take(Take);

        return page;
    }

    public async Task<IPage<IPost>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var totalCount = await db.Posts.CountAsync(Cancel).ConfigureAwait(false);
        
        if (PageSize == 0)
            return new Page<IPost>(Enumerable.Empty<IPost>(), PageIndex, PageSize, totalCount);
        
        var posts = await GetAllPostsByUserIdAsync(UserId, Cancel);

        return new Page<IPost>(posts, PageIndex, PageSize, totalCount);
    }

    #endregion

    public async Task<IPost?> GetPostAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == Id, Cancel).ConfigureAwait(false);

        return post as IPost;
    }

    public async Task<IPost> CreatePostAsync(
        string Title,
        string Body,
        string UserId,
        string Category,
        CancellationToken Cancel = default)
    {
        var post = new Post { Title = Title,
                              Body = Body,
                              CategoryName = Category, 
                              UserId = int.Parse(UserId) };

        await db.Posts.AddAsync(post).ConfigureAwait(false);
        await db.SaveChangesAsync(Cancel);

        return (IPost)post;
    }

    public async Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
    {
        var dbPost = await GetPostAsync(Id, Cancel);
        
        if(dbPost is null)
            return false;

        db.Remove(dbPost);
        await db.SaveChangesAsync(Cancel);

        return true;
    }

    #region Tags

    public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null || Tag == null)
            return false;

        var assignedTags = post.Tags;

        if(assignedTags != null && assignedTags.Any(n => n.Name == Tag)) 
            return true;

        var newTag = new Tag() { Name = Tag };

        post.Tags.Add((ITag)newTag);

        await db.SaveChangesAsync(Cancel);

        return true;
    }

    public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null || Tag == null)
            return false;

        var assignedTags = post.Tags;

        if (assignedTags is null || !assignedTags.Any(n => n.Name == Tag))
            return false;

        var removingTag = new Tag() { Name = Tag };

        assignedTags.Remove((ITag)removingTag);

        await db.SaveChangesAsync(Cancel);

        return false;
    }

    public async Task<IEnumerable<ITag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(Id, Cancel);
        if (post == null) throw new NotImplementedException();

        var assignedTags = post.Tags;
        
        return assignedTags;
    }

    public Task<IEnumerable<IPost>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        var searchingTag = new Tag() { Name = Tag };

        var searchingPosts = GetAllPostsAsync(Cancel)
            .Result
            .Where(t => t.Tags.Contains((ITag)searchingTag));

        return (Task<IEnumerable<IPost>>)searchingPosts;
    }

    #endregion

    #region Редактирование

    public async Task<ICategory> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null) { throw new NotImplementedException(); }

        post.Category.Name = CategoryName;
        await db.SaveChangesAsync(Cancel);

        return post.Category;
    }

    public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);
        
        if(post == null)
            return false;
        
        post.Title = Title;
        await db.SaveChangesAsync(Cancel);
        
        return true;
    }

    public async Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null)
            return false;

        post.Body = Body;
        await db.SaveChangesAsync(Cancel);

        return true;
    }

    public async Task<IStatus> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null) { throw new NotImplementedException(); }

        post.Status.Name = Status;
        await db.SaveChangesAsync(Cancel);

        return post.Status;
    }

    #endregion
}
