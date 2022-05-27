using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.TestConsole.Services;

public class InMemoryBlogPostManager : IBlogPostManager
{
    public Task<IEnumerable<IPost>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        if (Cancel.IsCancellationRequested)
            return Task.FromCanceled<IEnumerable<IPost>>(Cancel);
        return Task.FromResult(Enumerable.Empty<IPost>());
    }

    public Task<IEnumerable<IPost>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IPage<IPost>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<IPost>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<IPost>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IPage<IPost>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IPost?> GetPostAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IPost> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<ITag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<IPost>> GetPostsByTag(string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<ICategory> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IStatus> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default) => throw new NotImplementedException();
}
