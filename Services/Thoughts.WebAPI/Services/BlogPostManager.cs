
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
        /// <param name="Tag">Добавляемый тэг</param>
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
        /// <param name="Body">Новое тело поста</param>
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
        /// <param name="CategoryName">Новая категория поста</param>
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

        /// <summary>Изменение статуса поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Status">Новый статус поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Изменённая категория</returns>
        public Task<IStatus> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default)
        {
            var result = new Task<IStatus>(() =>
            {
                var existTask = _repo.ExistId(PostId, Cancel);
                var status = new Status(Status);
                if (existTask.Result is true)
                {
                    var gettedPost = _repo.GetById(PostId).Result;

                    gettedPost.Status = (IStatus)status;

                    var post = _repo.Update(gettedPost, Cancel);
                    if (post.Result is not null)
                    {
                        return (IStatus)status;
                    }
                }
                return null;
            });
            return result;
        }

        /// <summary>Изменение заголовка поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Title">Новый заголовок поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если заголовок был изменен успешно</returns>
        public Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
        {
            var result = new Task<bool>(() =>
            {
                var existTask = _repo.ExistId(PostId, Cancel);
                if (existTask.Result is true)
                {
                    var gettedPost = _repo.GetById(PostId).Result;

                    gettedPost.Title = Title;

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

        /// <summary>Создание нового поста (есть TODO блок)</summary>
        /// <param name="Title">Заголовок</param>
        /// <param name="Body">Тело</param>
        /// <param name="UserId">Идентификатор пользователя, создающего пост</param>
        /// <param name="Category">Категория</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Вновь созданный пост</returns>
        public Task<IPost> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default)
        {
            var newPost = new Post()
            {
                Title = Title,
                Body = Body,
                Category = new Category(Category),
                //
                //TODO
                //здесь нам как-то нужно добавить юзера
                //
            };
            return _repo.Add((IPost)newPost, Cancel);
        }

        /// <summary>Удаление поста</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если пост был удалён успешно</returns>
        public Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
        {
            var result = new Task<bool>(() =>
            {
                var existTask = _repo.ExistId(Id, Cancel);
                if (existTask.Result is true)
                {
                    var delete = _repo.DeleteById(Id);
                    if (delete.Result is not null)
                    {
                        return true;
                    }
                }
                return false;
            });
            return result;
        }
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
