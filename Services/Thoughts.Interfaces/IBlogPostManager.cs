using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Thoughts.Domain.Base.Entities;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Interfaces;

public interface IBlogPostManager
{
    #region Get all posts
    
    Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default);

    Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default);

    Task<IEnumerable<Post>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default);

    Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default);

    #endregion

    #region Get user posts
    
    Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default);

    Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default);

    Task<IEnumerable<Post>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default);

    Task<IPage<Post>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default);

    #endregion

    Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default);

    Task<Post> CreatePostAsync(
        string Title,
        string Body,
        string UserId,
        string Category,
        CancellationToken Cancel = default);

    Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default);

    #region Tags

    Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default);

    Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default);

    Task<IEnumerable<Tag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default);

    Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default);

    #endregion

    #region Редактирование
    
    Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default);

    Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default);

    Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default);

    #endregion
}

public static class BlogPostManagerExtensions
{
    public static async Task<bool> EditPostAsync(
        this IBlogPostManager Posts, 
        int PostId, 
        string? NewBody, 
        string? NewTitle = null, 
        CancellationToken Cancel = default)
    {
        if(NewBody is { } body)
            if (!await Posts.ChangePostBodyAsync(PostId, body, Cancel).ConfigureAwait(false))
                return false;

        if(NewTitle is { } title)
            if (!await Posts.ChangePostTitleAsync(PostId, title, Cancel).ConfigureAwait(false))
                return false;

        return true;
    }
}