using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.DAL.Entities;

namespace Thoughts.WebAPI.Services
{
    public class BlogPostManager : IBlogPostManager
    {
        private readonly IRepository<IPost> _repo;

        public BlogPostManager(IRepository<IPost> repository)
        {
            _repo = repository;
        }
        /// <summary>Назначение тэга посту</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Tag">Тэг</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тэг был назначен успешно</returns>
        public Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
        {
            var result = new Task<bool>(() =>
            {
                var existTask = _repo.ExistId(PostId, Cancel);
                var tag = new Tag(Tag);
                if (existTask.Result is true)
                {
                    var gettedPost = _repo.GetById(PostId).Result;

                    //хз почему, но не мог добавить экземпляр Тэга без явного приведения
                    gettedPost.Tags.Add((ITag)tag);

                    var post = _repo.Update(gettedPost, Cancel);
                    if (post.Result is not null)
                    {
                        return true;
                    }
                }
                return false;
            });
            return result;

        }

        /// <summary>Изменение тела поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Body">Тело поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тело было изменено успешно</returns>
        public Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
        {
            var result = new Task<bool>(() =>
            {
                var existTask = _repo.ExistId(PostId, Cancel);
                if (existTask.Result is true)
                {
                    var gettedPost = _repo.GetById(PostId).Result;

                    gettedPost.Body = Body;

                    var post = _repo.Update(gettedPost, Cancel);
                    if (post.Result is not null)
                    {
                        return true;
                    }
                }
                return false;
            });
            return result;
        }

        /// <summary>Изменение категории поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="CategoryName">Категория поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Изменённая категория</returns>
        public Task<ICategory> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
        {
            var result = new Task<ICategory>(() =>
            {
                var existTask = _repo.ExistId(PostId, Cancel);
                var category = new Category(CategoryName);
                if (existTask.Result is true)
                {
                    var gettedPost = _repo.GetById(PostId).Result;

                    gettedPost.Category = (ICategory)category;

                    var post = _repo.Update(gettedPost, Cancel);
                    if (post.Result is not null)
                    {
                        return (ICategory)category;
                    }
                }
                return null;
            });
            return result;
        }
        public Task<IStatus> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IPost> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IEnumerable<IPost>> GetAllPostsAsync(CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IEnumerable<IPost>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IPage<IPost>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IEnumerable<IPost>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IPage<IPost>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IEnumerable<IPost>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IEnumerable<ITag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IPost?> GetPostAsync(int Id, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<IEnumerable<IPost>> GetPostsByTag(string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default) => throw new NotImplementedException();
        public Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default) => throw new NotImplementedException();
    }
}
