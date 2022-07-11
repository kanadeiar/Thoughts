using Thoughts.Domain;
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

using Post = Thoughts.Domain.Base.Entities.Post;
using Tag = Thoughts.Domain.Base.Entities.Tag;
using Category = Thoughts.Domain.Base.Entities.Category;
using User = Thoughts.Domain.Base.Entities.User;

namespace Thoughts.WebAPI.Services
{
    public class BlogPostManager : IBlogPostManager
    {
        private readonly IRepository<Post> _PostsRepository;
        private readonly INamedRepository<Category> _CategoriesRepository;
        private readonly INamedRepository<Tag> _TagsRepository;
        private readonly IRepository<User, string> _UsersRepository;
        private readonly ILogger<BlogPostManager> _Logger;

        public BlogPostManager(
            IRepository<Post> PostsRepository,
            INamedRepository<Category> CategoriesRepository,
            INamedRepository<Tag> TagsRepository,
            IRepository<User, string> UsersRepository,
            ILogger<BlogPostManager> Logger)
        {
            _PostsRepository = PostsRepository;
            _CategoriesRepository = CategoriesRepository;
            _TagsRepository = TagsRepository;
            _UsersRepository = UsersRepository;
            _Logger = Logger;
        }

        /// <summary>Назначение тэга посту</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Tag">Добавляемый тэг</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Истина, если тэг был назначен успешно</returns>
        public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
        {
            if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
                return false;

            if (await _TagsRepository.GetByName(Tag, Cancel) is not { } tag) 
                tag = await _TagsRepository.Add(new Tag { Name = Tag }, Cancel);

            if (post.Tags.Contains(tag.Id))
                return true;

            post.Tags.Add(tag.Id);

            await _PostsRepository.Update(post, Cancel);

            return true;
        }

        /// <summary>Изменение тела поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Body">Новое тело поста</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Истина, если тело было изменено успешно</returns>
        public async Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
        {
            if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
                return false;

            if (Equals(post.Body, Body))
                return true;

            post.Body = Body;

            await _PostsRepository.Update(post, Cancel);
            return true;
        }

        /// <summary>Изменение категории поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="CategoryName">Новая категория поста</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Изменённая категория</returns>
        public async Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
        {
            if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
                throw new InvalidOperationException("Не найдена запись поста с указанным идентификатором");

            if (await _CategoriesRepository.GetByName(CategoryName, Cancel) is not { } category)
                category = await _CategoriesRepository.Add(new() { Name = CategoryName }, Cancel);

            if (post.Category.Id == category.Id)
                return category;

            post.Category = (category.Id, CategoryName);

            await _PostsRepository.Update(post, Cancel);
            return category;
        }

        /// <summary>Изменение заголовка поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Title">Новый заголовок поста</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Истина, если заголовок был изменен успешно</returns>
        public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
        {
            if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
                return false;

            if (Equals(post.Title, Title))
                return true;

            post.Title = Title;

            await _PostsRepository.Update(post, Cancel);
            return true;
        }

        /// <summary>Создание нового поста (есть TODO блок)</summary>
        /// <param name="Title">Заголовок</param>
        /// <param name="Body">Тело</param>
        /// <param name="UserId">Идентификатор пользователя, создающего пост</param>
        /// <param name="CategoryName">Категория</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Вновь созданный пост</returns>
        public async Task<Post> CreatePostAsync(string Title, string Body, string UserId, string CategoryName, CancellationToken Cancel = default)
        {
            if (await _UsersRepository.GetById(UserId, Cancel) is not { } user)
                throw new InvalidOperationException($"Пользователь с id {UserId} не найден");

            if (await _CategoriesRepository.GetByName(CategoryName, Cancel) is not { } category)
                category = await _CategoriesRepository.Add(new() { Name = CategoryName }, Cancel);

            var post = new Post
            {
                Title = Title,
                Body = Body,
                Category = (category.Id, category.Name),
                User = user,
            };

            return await _PostsRepository.Add(post, Cancel);
        }

        /// <summary>Удаление поста</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Истина, если пост был удалён успешно</returns>
        public async Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
        {
            var deleted_post = await _PostsRepository.DeleteById(Id, Cancel).ConfigureAwait(false);
            return deleted_post is not null;
        }

        /// <summary>Получить все посты</summary>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Перечисление всех постов</returns>
        public async Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default) => await _PostsRepository
           .GetAll(Cancel)
           .ConfigureAwait(false);

        /// <summary>Получить все посты пользователя по его идентификатору</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Перечисление всех постов пользователя</returns>
        public async Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
        {
            var posts = await _PostsRepository.GetAll(Cancel); // todo: Требуется добавить возможность выборки поста в репозитории IPostsRepository : INamedRepository<Post>
            return posts.Where(p => p.User.Id == UserId);
        }

        /// <summary>Получить все страницы с постами пользователя по его идентификатору</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="PageIndex">Номер страницы</param>
        /// <param name="PageSize">Размер страницы</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Страница с перечислением всех постов пользователя</returns>
        public async Task<IPage<Post>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var posts = await _PostsRepository.GetAll(Cancel); // todo: Требуется добавить возможность выборки поста в репозитории IPostsRepository : INamedRepository<Post>

            var user_posts = posts.Where(p => p.User.Id == UserId);

            var total_count = user_posts.Count();
            var page_items = user_posts.Skip(PageIndex * PageSize).Take(PageSize);

            return new Page<Post>(page_items, PageIndex, PageSize, total_count);
        }

        /// <summary>Получить определённое количество постов пользователя</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Skip">Количество пропускаемых элементов</param>
        /// <param name="Take">Количество получаемых элементов</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Перечисление постов пользователя</returns>
        public async Task<IEnumerable<Post>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
        {
            var posts = await GetAllPostsAsync(Cancel);
            return posts.Where(p => p.User.Id == UserId).Skip(Skip).Take(Take);
        }

        /// <summary>Получить число постов</summary>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Число постов</returns>
        public async Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default) => await _PostsRepository
           .GetCount(Cancel)
           .ConfigureAwait(false);

        /// <summary>Получить страницу со всеми постами</summary>
        /// <param name="PageIndex">Номер страницы</param>
        /// <param name="PageSize">Размер страницы</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Страница с постами</returns>
        public async Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default) =>
            await _PostsRepository.GetPage(PageIndex, PageSize, Cancel)
               .ConfigureAwait(false);

        /// <summary>Получить определённое количество постов из всех</summary>
        /// <param name="Skip">Количество пропускаемых элементов</param>
        /// <param name="Take">Количество получаемых элементов</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Перечисление постов</returns>
        public async Task<IEnumerable<Post>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
        {
            var posts = await GetAllPostsAsync(Cancel);
            return posts.Skip(Skip).Take(Take);
        }

        /// <summary>Получить тэги к посту по его идентификатору</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Перечисление тэгов</returns>
        public async Task<IEnumerable<Tag>> GetBlogTagsAsync(int PostId, CancellationToken Cancel = default)
        {
            if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
                throw new InvalidOperationException($"Пост с id {PostId} не найден");

            var tag_ids = post.Tags;

            var tags = new List<Tag>();
            foreach (var tag_id in tag_ids)
            {
                var tag = await _TagsRepository.GetById(tag_id, Cancel);
                tags.Add(tag);
            }

            return tags;
        }

        /// <summary>Получение поста по его идентификатору</summary>
        /// <param name="Id">Идентификатор поста</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Найденный пост или <b>null</b></returns>
        public async Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default) => await _PostsRepository
           .GetById(Id, Cancel)
           .ConfigureAwait(false);

        /// <summary>Получить посты по тэгу</summary>
        /// <param name="Tag">Тэг</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Перечисление постов</returns>
        public async Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
        {
            if (await _TagsRepository.GetByName(Tag, Cancel).ConfigureAwait(false) is not { Id: var tag_id } tag)
                return Enumerable.Empty<Post>();

            var posts = await _PostsRepository.GetAll(Cancel);

            var tags_posts = posts.Where(p => p.Tags.Contains(tag_id));
            return tags_posts;
        }

        /// <summary>Получить количество постов пользователя</summary>
        /// <param name="UserId">Идентификатор пользователя</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Количество постов</returns>
        public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
        {
            var posts = await _PostsRepository.GetAll(Cancel);
            return posts.Count(p => p.User.Id == UserId);
        }

        /// <summary>Удалить тэг с поста</summary>
        /// <param name="PostId">Идентификатор поста</param>
        /// <param name="Tag">Тэг</param>
        /// <param name="Cancel">Отмена асинхронной операции</param>
        /// <returns>Истина, если тэг был удалён успешно</returns>
        public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
        {
            if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
                throw new InvalidOperationException($"Пост с id {PostId} не найден");

            if (await _TagsRepository.GetByName(Tag, Cancel) is not { Id: var tag_id } tag)
                return false;

            if (!post.Tags.Remove(tag_id))
                return false;

            await _PostsRepository.Update(post, Cancel);

            return true;
        }
    }
}
