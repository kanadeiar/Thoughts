using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.DAL.Entities;

using Post = Thoughts.Domain.Base.Entities.Post;
using Tag = Thoughts.Domain.Base.Entities.Tag;
using Category = Thoughts.Domain.Base.Entities.Category;
using System.Linq;

namespace Thoughts.WebAPI.Services
{
    public class BlogPostManager : IBlogPostManager
    {
        private readonly IRepository<Post> _PostsRepository;

        public BlogPostManager(IRepository<Post> repository) => _PostsRepository = repository;

        /// <summary>Назначение тэга посту</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Tag">Добавляемый тэг</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тэг был назначен успешно</returns>
        public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(PostId, Cancel))
                return false;

            var getted_post = await _PostsRepository.GetById(PostId, Cancel);

            var tag = new Tag { Name = Tag };
            getted_post.Tags.Add(tag);
            await _PostsRepository.Update(getted_post, Cancel);

            return true;
        }

        /// <summary>Изменение тела поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Body">Новое тело поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тело было изменено успешно</returns>
        public async Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(PostId, Cancel))
                return false;

            var getted_post = await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false);

            getted_post.Body = Body;

            await _PostsRepository.Update(getted_post, Cancel);
            return true;
        }

        /// <summary>Изменение категории поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="CategoryName">Новая категория поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Изменённая категория</returns>
        public async Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(PostId, Cancel))
                return null;

            var getted_post = await _PostsRepository.GetById(PostId, Cancel);

            var category = new Category { Name = CategoryName };
            getted_post.Category = category;

            await _PostsRepository.Update(getted_post, Cancel);
            return category;
        }

        /// <summary>Изменение заголовка поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Title">Новый заголовок поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если заголовок был изменен успешно</returns>
        public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(PostId, Cancel))
                return false;

            var getted_post = await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false);

            getted_post.Title = Title;

            await _PostsRepository.Update(getted_post, Cancel);
            return true;
        }

        /// <summary>Создание нового поста (есть TODO блок)</summary>
        /// <param name="Title">Заголовок</param>
        /// <param name="Body">Тело</param>
        /// <param name="UserId">Идентификатор пользователя, создающего пост</param>
        /// <param name="Category">Категория</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Вновь созданный пост</returns>
        public Task<Post> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default)
        {
            var new_post = new Post
            {
                Title = Title,
                Body = Body,
                Category = new Category { Name = Category },
                //
                //TODO
                //здесь нам как-то нужно добавить юзера
                //
            };
            return _PostsRepository.Add(new_post, Cancel);
        }

        /// <summary>Удаление поста</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если пост был удалён успешно</returns>
        public async Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(Id, Cancel))
                return false;

            await _PostsRepository.DeleteById(Id, Cancel);
            return true;
        }

        /// <summary>Получить все посты</summary>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление всех постов</returns>
        public async Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default) => await _PostsRepository
           .GetAll(Cancel)
           .ConfigureAwait(false);

        /// <summary>Получить все посты пользователя по его идентификатору</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление всех постов пользователя</returns>
        public async Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
        {
            var posts = await GetAllPostsAsync(Cancel);
            return posts.Where(p => p.User.Id == UserId);
        }

        /// <summary>Получить все страницы с постами пользователя по его идентификатору (есть TODO блок)</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="PageIndex">Номер страницы</param>
        /// <param name="PageSize">Размер страницы</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Страница с перечислением всех постов пользователя</returns>
        public async Task<IPage<Post>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var pages = await _PostsRepository.GetPage(PageIndex, PageSize, Cancel);

            //
            // TODO
            // интерфейс не позволяет менять свойство Items
            // а также нет сущности, реализующей интерфейс страницы
            // поэтому метод временно возвращает все страницы
            //
            pages.Items.Where(p => p.User.Id == UserId);

            return pages;
        }

        /// <summary>Получить определённое количество постов пользователя</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Skip">Количество пропускаемых элементов</param>
        /// <param name="Take">Количество получаемых элементов</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление постов пользователя</returns>
        public async Task<IEnumerable<Post>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
        {
            var posts = await GetAllPostsAsync(Cancel);
            return posts.Where(p => p.User.Id == UserId).Skip(Skip).Take(Take);
        }

        /// <summary>Получить число постов</summary>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Число постов</returns>
        public async Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default) => await _PostsRepository
           .GetCount(Cancel)
           .ConfigureAwait(false);

        /// <summary>Получить страницу со всеми постами</summary>
        /// <param name="PageIndex">Номер страницы</param>
        /// <param name="PageSize">Размер страницы</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Страница с постами</returns>
        public async Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default) =>
            await _PostsRepository.GetPage(PageIndex, PageSize, Cancel)
               .ConfigureAwait(false);

        /// <summary>Получить определённое количество постов из всех</summary>
        /// <param name="Skip">Количество пропускаемых элементов</param>
        /// <param name="Take">Количество получаемых элементов</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление постов</returns>
        public async Task<IEnumerable<Post>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
        {
            var posts = await GetAllPostsAsync(Cancel);
            return posts.Skip(Skip).Take(Take);
        }

        /// <summary>Получить тэги к посту по его идентификатору</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление тэгов</returns>
        public async Task<IEnumerable<Tag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(Id, Cancel))
                return null;

            var tags = await _PostsRepository.GetById(Id, Cancel);
            return tags.Tags;
        }

        /// <summary>Получение поста по его идентификатору</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Найденный пост или <b>null</b></returns>
        public async Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(Id, Cancel))
                return null;

            return await _PostsRepository.GetById(Id, Cancel);
        }

        /// <summary>Получить посты по тэгу</summary>
        /// <param name="Tag">Тэг</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Перечисление постов</returns>
        public async Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
        {
            var posts = await _PostsRepository.GetAll(Cancel);

            var tag = new Tag { Name = Tag };
            var tags_posts = posts.Where(p => p.Tags.Contains(tag));
            return tags_posts;
        }

        /// <summary>Получить количество постов пользователя</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Количество постов</returns>
        public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
        {
            var posts = await _PostsRepository.GetAll(Cancel);
            return posts.Count(p => p.User.Id == UserId);
        }

        /// <summary>Удалить тэг с поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Tag">Тэг</param>
        /// <param name="Cancel">Токен отмены</param>
        /// <returns>Истина, если тэг был удалён успешно</returns>
        public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
        {
            if (!await _PostsRepository.ExistId(PostId, Cancel))
                return false;

            var getted_post = await _PostsRepository.GetById(PostId, Cancel);
            if (getted_post is not null)
            {
                var post = getted_post;
                var tag = new Tag { Name = Tag };
                return post.Tags.Remove(tag);
            }
            return false;
        }
    }
}
