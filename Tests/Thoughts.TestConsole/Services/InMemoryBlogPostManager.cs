using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

using Post = Thoughts.Domain.Base.Entities.Post;
using Tag = Thoughts.Domain.Base.Entities.Tag;
using Category = Thoughts.Domain.Base.Entities.Category;

namespace Thoughts.TestConsole.Services;

public class InMemoryBlogPostManager : IBlogPostManager
{
    public Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        if (Cancel.IsCancellationRequested)
            return Task.FromCanceled<IEnumerable<Post>>(Cancel);
        return Task.FromResult(Enumerable.Empty<Post>());
    }

    public Task<IEnumerable<Post>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<Post>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IPage<Post>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<Post> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<Tag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();
    public async Task<IEnumerable<Tag>> GetMostPopularTags() => throw new NotImplementedException();

    public Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default) => throw new NotImplementedException();

    public Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default) => throw new NotImplementedException();
}
