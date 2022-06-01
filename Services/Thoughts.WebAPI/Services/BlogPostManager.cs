
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.DAL.Entities;

namespace Thoughts.WebAPI.Services
{
    public class BlogPostManager : IBlogPostManager
    {
        private readonly IRepository<IPost> _PostsRepository;

        public BlogPostManager(IRepository<IPost> repository)
        {
            _PostsRepository = repository;
        }

        /// <summary>Назначение тэга посту</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Tag">Добавляемый тэг</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тэг был назначен успешно</returns>
        public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
        {
            var is_exists = await _PostsRepository.ExistId(PostId, Cancel);
            if (!is_exists) return false;

            var getted_post = await _PostsRepository.GetById(PostId, Cancel);

            //хз почему, но не мог добавить экземпляр сущностей без явного приведения
            var tag = new Tag(Tag);
            getted_post.Tags.Add((ITag)tag);
            await _PostsRepository.Update(getted_post, Cancel);

            return true;
        }

        /// <summary>Изменение тела поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Body">Новое тело поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тело было изменено успешно</returns>
        public Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
        {
            var task = new Task<bool>(() =>
            {
                var exist_task = _PostsRepository.ExistId(PostId, Cancel);
                if (exist_task.Result is true)
                {
                    var getted_post = _PostsRepository.GetById(PostId).Result;

                    getted_post.Body = Body;

                    var post = _PostsRepository.Update(getted_post, Cancel);
                    if (post.Result is not null)
                    {
                        return true;
                    }
                }
                return false;
            });
            return task;
        }

        /// <summary>Изменение категории поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="CategoryName">Новая категория поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Изменённая категория</returns>
        public Task<ICategory> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
        {
            var task = new Task<ICategory>(() =>
            {
                var exist_task = _PostsRepository.ExistId(PostId, Cancel);
                var category = new Category(CategoryName);
                if (exist_task.Result is true)
                {
                    var getted_post = _PostsRepository.GetById(PostId).Result;

                    getted_post.Category = (ICategory)category;

                    var post = _PostsRepository.Update(getted_post, Cancel);
                    if (post.Result is not null)
                    {
                        return (ICategory)category;
                    }
                }
                return null;
            });
            return task;
        }

        /// <summary>Изменение статуса поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Status">Новый статус поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Изменённая категория</returns>
        public Task<IStatus> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default)
        {
            var task = new Task<IStatus>(() =>
            {
                var exist_task = _PostsRepository.ExistId(PostId, Cancel);
                var status = new Status(Status);
                if (exist_task.Result is true)
                {
                    var getted_post = _PostsRepository.GetById(PostId).Result;

                    getted_post.Status = (IStatus)status;

                    var post = _PostsRepository.Update(getted_post, Cancel);
                    if (post.Result is not null)
                    {
                        return (IStatus)status;
                    }
                }
                return null;
            });
            return task;
        }

        /// <summary>Изменение заголовка поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Title">Новый заголовок поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если заголовок был изменен успешно</returns>
        public Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
        {
            var task = new Task<bool>(() =>
            {
                var exist_task = _PostsRepository.ExistId(PostId, Cancel);
                if (exist_task.Result is true)
                {
                    var getted_post = _PostsRepository.GetById(PostId).Result;

                    getted_post.Title = Title;

                    var post = _PostsRepository.Update(getted_post, Cancel);
                    if (post.Result is not null)
                    {
                        return true;
                    }
                }
                return false;
            });
            return task;
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
            var new_post = new Post()
            {
                Title = Title,
                Body = Body,
                Category = new Category(Category),
                //
                //TODO
                //здесь нам как-то нужно добавить юзера
                //
            };
            return _PostsRepository.Add((IPost)new_post, Cancel);
        }

        /// <summary>Удаление поста</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если пост был удалён успешно</returns>
        public Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
        {
            var task = new Task<bool>(() =>
            {
                var exist_task = _PostsRepository.ExistId(Id, Cancel);
                if (exist_task.Result is true)
                {
                    var delete = _PostsRepository.DeleteById(Id);
                    if (delete.Result is not null)
                    {
                        return true;
                    }
                }
                return false;
            });
            return task;
        }

        /// <summary>Получить все посты</summary>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление всех постов</returns>
        public Task<IEnumerable<IPost>> GetAllPostsAsync(CancellationToken Cancel = default)
        {
            return _PostsRepository.GetAll(Cancel);
        }

        /// <summary>Получить все посты пользователя по его идентификатору</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление всех постов пользователя</returns>
        public Task<IEnumerable<IPost>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
        {
            var task = new Task<IEnumerable<IPost>>(() =>
            {
                return GetAllPostsAsync(Cancel).Result.Where(p => p.User.Id == UserId);
            });
            return task;
        }

        /// <summary>Получить все страницы с постами пользователя по его идентификатору (есть TODO блок)</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="PageIndex">Номер страницы</param>
        /// <param name="PageSize">Размер страницы</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Страница с перечислением всех постов пользователя</returns>
        public Task<IPage<IPost>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var task = new Task<IPage<IPost>>(() =>
            {
                var pages = _PostsRepository.GetPage(PageIndex, PageSize, Cancel).Result;

                //
                // TODO
                // интерфейс не позволяет менять свойство Items
                // а также нет сущности, реализующей интерфейс страницы
                // поэтому метод временно возвращает все страницы
                //
                pages.Items.Where(p => p.User.Id == UserId);

                return pages;
            });
            return task;
        }

        /// <summary>Получить определённое количество постов пользователя</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Skip">Количество пропускаемых элементов</param>
        /// <param name="Take">Количество получаемых элементов</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление постов пользователя</returns>
        public Task<IEnumerable<IPost>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
        {
            var task = new Task<IEnumerable<IPost>>(() =>
            {
                return GetAllPostsAsync(Cancel).Result.Where(p => p.User.Id == UserId).Skip(Skip).Take(Take);
            });
            return task;
        }

        /// <summary>Получить число постов</summary>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Число постов</returns>
        public Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default)
        {
            return _PostsRepository.GetCount(Cancel);
        }

        /// <summary>Получить страницу со всеми постами</summary>
        /// <param name="PageIndex">Номер страницы</param>
        /// <param name="PageSize">Размер страницы</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Страница с постами</returns>
        public Task<IPage<IPost>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            return _PostsRepository.GetPage(PageIndex, PageSize, Cancel);
        }

        /// <summary>Получить определённое количество постов из всех</summary>
        /// <param name="Skip">Количество пропускаемых элементов</param>
        /// <param name="Take">Количество получаемых элементов</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление постов</returns>
        public Task<IEnumerable<IPost>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
        {
            var task = new Task<IEnumerable<IPost>>(() =>
            {
                return GetAllPostsAsync(Cancel).Result.Skip(Skip).Take(Take);
            });
            return task;
        }

        /// <summary>Получить тэги к посту по его идентификатору</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление тэгов</returns>
        public Task<IEnumerable<ITag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
        {
            var task = new Task<IEnumerable<ITag>>(() =>
            {
                var exist_task = _PostsRepository.ExistId(Id, Cancel);
                if (exist_task.Result is true)
                {
                    return _PostsRepository.GetById(Id).Result.Tags;
                }
                return null;
            });
            return task;
        }

        /// <summary>Получение поста по его идентификатору</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Найденный пост или <b>null</b></returns>
        public Task<IPost?> GetPostAsync(int Id, CancellationToken Cancel = default)
        {
            var task = new Task<IPost>(() =>
            {
                var exist_task = _PostsRepository.ExistId(Id, Cancel);
                if (exist_task.Result is true)
                {
                    return _PostsRepository.GetById(Id).Result;
                }
                return null;
            });
            return task;
        }

        /// <summary>Получить посты по тэгу</summary>
        /// <param name="Tag">Тэг</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление постов</returns>
        public Task<IEnumerable<IPost>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
        {
            var tag = new Tag(Tag);
            var task = new Task<IEnumerable<IPost>>(() =>
            {
                var posts = _PostsRepository.GetAll(Cancel).Result.Where(p => p.Tags.Contains((ITag)tag));
                return posts;
            });
            return task;
        }

        /// <summary>Получить количество постов пользователя</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Количество постов</returns>
        public Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
        {
            var task = new Task<int>(() =>
            {
                return _PostsRepository.GetAll().Result.Where(p => p.User.Id == UserId).Count();
            });
            return task;
        }

        /// <summary>Удалить тэг с поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Tag">Тэг</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тэг был удалён успешно</returns>
        public Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
        {
            var tag = new Tag(Tag);

            var task = new Task<bool>(() =>
            {
                var post_exist = _PostsRepository.ExistId(PostId, Cancel);
                if(post_exist.Result is true)
                {
                    var getted_post = _PostsRepository.GetById(PostId, Cancel);
                    if(getted_post.Result is not null)
                    {
                        var post = getted_post.Result;
                        return post.Tags.Remove((ITag)tag);
                    }
                }
                return false;
            });
            return task;
        }
    }
}
