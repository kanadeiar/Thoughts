using Microsoft.Extensions.Logging;

using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

using Thoughts.Domain;
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Services;

public class RepositoryBlogPostManager : IBlogPostManager
{
    private readonly IRepository<Post> _PostsRepository;
    private readonly INamedRepository<Tag> _TagsRepository;
    private readonly INamedRepository<Category> _CategoriesRepository;
    private readonly IRepository<User, string> _UsersRepository;
    private readonly ILogger<RepositoryBlogPostManager> _Logger;

    public RepositoryBlogPostManager(
        IRepository<Post> PostsRepository,
        INamedRepository<Tag> TagsRepository,
        INamedRepository<Category> CategoriesRepository,
        IRepository<User, string> UsersRepository,
        ILogger<RepositoryBlogPostManager> Logger)
    {
        _PostsRepository = PostsRepository;
        _TagsRepository = TagsRepository;
        _CategoriesRepository = CategoriesRepository;
        _UsersRepository = UsersRepository;
        _Logger = Logger;
    }

    #region Get All Posts

    /// <summary>Получить все записи блогов</summary>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Возвращает все записи блогов</returns>
    public async Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        var posts = await _PostsRepository.GetAll(Cancel).ConfigureAwait(false);
        return posts;
    }

    /// <summary>Получить количество всех записей блогов</summary>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Возвращает количество записей блогов</returns>
    public async Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default)
    {
        var posts_count = await _PostsRepository.GetCount(Cancel).ConfigureAwait(false);
        return posts_count;
    }

    /// <summary>Получить записи с возможностью пропуска записей в начале и выборки заданного количества</summary>
    /// <param name="Skip">Пропуск количества заданного диапазона записей блогов</param>
    /// <param name="Take">Получение заданного диапазона записей блогов</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Выборка записей блогов</returns>
    public async Task<IEnumerable<Post>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        if (Take == 0)
            return Enumerable.Empty<Post>();

        var page = await _PostsRepository.Get(Skip, Take, Cancel).ConfigureAwait(false);

        return page;
    }

    /// <summary>Получение страницы записей блогов</summary>
    /// <param name="PageIndex">Номер страницы</param>
    /// <param name="PageSize">Размер страницы</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Страница записей блогов</returns>
    public async Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        if (PageSize == 0)
        {
            var total_count = await _PostsRepository.GetCount(Cancel).ConfigureAwait(false);
            return new Page<Post>(Enumerable.Empty<Post>(), PageIndex, PageSize, total_count);
        }

        var page = await _PostsRepository.GetPage(PageIndex, PageSize, Cancel).ConfigureAwait(false);

        //здесь не уверен, всё же в конструкторе страницы обязательно указывать общее количество, а в интерфейсе репозитория общее количество не указывается
        return page;
    }

    #endregion

    #region Get All Posts By User

    /// <summary>Получение всех записей блогов пользователя</summary>
    /// <param name="UserId">ID пользователя</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Все пользовательские записи блогов</returns>
    public async Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        var all_posts = await _PostsRepository.GetAll(Cancel);

        var user_posts = all_posts.Where(p => p.User.Id == UserId);
        return user_posts;
    }

    /// <summary>Получение количества всех записей блогов пользователя</summary>
    /// <param name="UserId">ID пользователя</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Количество всех записей блогов пользователя</returns>
    public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        var all_posts = await GetAllPostsByUserIdAsync(UserId, Cancel).ConfigureAwait(false);

        var count = all_posts.Count(); // todo: надо обучить репозиторий выдавать записи по id указанного пользователя

        return count;
    }

    /// <summary>Получение выборки записей блогов постраничного разбиения конкретного пользователя</summary>
    /// <param name="UserId">ID пользователя</param>
    /// <param name="Skip">Пропуск количества заданного диапазона записей блогов</param>
    /// <param name="Take">Получение заданного диапазона записей блогов</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Выборка записей блогов для постраничного разбиения пользователя</returns>
    public async Task<IEnumerable<Post>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
    {
        if (Take <= 0)
            return Enumerable.Empty<Post>();

        var all_posts_by_user_id = await GetAllPostsByUserIdAsync(UserId, Cancel);

        var page = all_posts_by_user_id.Skip(Skip).Take(Take);

        return page;
    }

    /// <summary>Получение страницы записей блогов пользователя</summary>
    /// <param name="UserId">ID пользователя</param>
    /// <param name="PageIndex">Номер страницы</param>
    /// <param name="PageSize">Размер страницы</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Страница записей блогов пользователя</returns>
    public async Task<IPage<Post>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        if (PageSize == 0)
        {
            var count = await GetUserPostsCountAsync(UserId, Cancel).ConfigureAwait(false);
            return new Page<Post>(Enumerable.Empty<Post>(), PageIndex, PageSize, count);
        }

        var all_posts = await _PostsRepository.GetAll(Cancel).ConfigureAwait(false);

        var user_posts = all_posts.Where(p => p.User.Id == UserId);
        var user_posts_count = user_posts.Count();

        var posts = user_posts
           .Skip(PageIndex * PageSize)
           .Take(PageSize);

        return new Page<Post>(posts, PageIndex, PageSize, user_posts_count);
    }

    #endregion

    #region Get Delete Create post

    /// <summary>Получение записи блога по его Id</summary>
    /// <param name="Id">Id записи блога</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Выбранная запись блога</returns>
    public async Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await _PostsRepository.GetById(Id, Cancel).ConfigureAwait(false);
        return post;
    }

    /// <summary>Удаление записи блога</summary>
    /// <param name="Id">Идентификатор записи блога</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns><c>true</c> - успешное удаление записи блога</returns>
    public async Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
    {
        var deleted_post = await _PostsRepository.DeleteById(Id, Cancel).ConfigureAwait(false);
        return deleted_post is not null;
    }

    /// <summary>Создание записи блога</summary>
    /// <param name="Title">Заголовок</param>
    /// <param name="Body">Тело записи блога</param>
    /// <param name="UserId">Id автора записи блога</param>
    /// <param name="Category">Категория записи блога</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Созданный пост</returns>
    public async Task<Post> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default)
    {
        if (Title is null) throw new ArgumentNullException(nameof(Title));
        if (Body is null) throw new ArgumentNullException(nameof(Body));
        if (UserId is not { Length: > 0 }) throw new ArgumentException("Не указан идентификатор пользователя", nameof(UserId));
        if (Category is null) throw new ArgumentNullException(nameof(Category));

        if (await _UsersRepository.GetById(UserId, Cancel).ConfigureAwait(false) is not { } user)
            throw new InvalidOperationException($"Пользователь с id {UserId} не найден");

        if (await _CategoriesRepository.GetByName(Category, Cancel) is not { } category)
            category = await _CategoriesRepository.Add(new() { Name = Category }, Cancel);

        var post = new Post
        {
            Title = Title,
            Body = Body,
            User = user,
            Category = (category.Id, category.Name),
        };

        return await _PostsRepository.Add(post, Cancel);
    }

    #endregion

    #region Tag

    /// <summary>Добавление ключевого слова к посту</summary>
    /// <param name="PostId">Идентификатор записи блога</param>
    /// <param name="Tag">Ключевое слово</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns><c>true</c> - ключевое слово успешно добавлено</returns>
    public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        if (Tag is null) throw new ArgumentNullException(nameof(Tag));

        if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
            throw new InvalidOperationException($"Пост с id {PostId} не найден");

        if (await _TagsRepository.GetByName(Tag, Cancel) is not { } tag)
            tag = await _TagsRepository.Add(new() { Name = Tag }, Cancel);

        if (post.Tags.Contains(tag.Id))
            return true;

        post.Tags.Add(tag.Id);

        await _PostsRepository.Update(post, Cancel);

        return true;
    }

    /// <summary>Удаление ключевого слова из записи блога</summary>
    /// <param name="PostId">Идентификатор записи блога</param>
    /// <param name="Tag">Ключевое слово</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns><c>true</c> - ключевое слово успешно удалено</returns>
    public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        if (Tag is null) throw new ArgumentNullException(nameof(Tag));

        if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
            throw new InvalidOperationException($"Пост с id {PostId} не найден");

        if (await _TagsRepository.GetByName(Tag, Cancel) is not { Id: var tag_id })
            return false;

        if (!post.Tags.Remove(tag_id))
            return false;

        await _PostsRepository.Update(post, Cancel);

        return true;
    }

    /// <summary>Получение всех ключевых слов записи блога</summary>
    /// <param name="PostId">Идентификатор записи блога</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Перечень ключевых слов записи блога</returns>
    /// <exception cref="InvalidOperationException">В случае если запись блога с указанным идентификатором не найдена</exception>
    public async Task<IEnumerable<Tag>> GetBlogTagsAsync(int PostId, CancellationToken Cancel = default)
    {
        if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
            throw new InvalidOperationException($"Пост с id {PostId} не найден");

        var tags = new List<Tag>(post.Tags.Count);
        foreach (var tag_id in post.Tags)
        {
            var tag = await _TagsRepository.GetById(tag_id, Cancel);
            tags.Add(tag);
        }

        return tags.ToArray();
    }

    /// <summary>Получение всех записей блогов по ключевому слову</summary>
    /// <param name="Tag">Ключевое слово</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Перечисление записей блогов с указанным ключевым словом</returns>
    public async Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        if (Tag is null) throw new ArgumentNullException(nameof(Tag));

        if (await _TagsRepository.GetByName(Tag, Cancel).ConfigureAwait(false) is not { Id: var tag_id })
            return Enumerable.Empty<Post>();

        var all_posts = await _PostsRepository.GetAll(Cancel);

        var tagged_posts = all_posts.Where(post => post.Tags.Contains(tag_id)).ToArray();

        return tagged_posts;
    }

    #endregion

    #region Edit

    /// <summary>Изменение заголовка записи блога</summary>
    /// <param name="PostId">Идентификатор записи блога</param>
    /// <param name="Title">Заголовок записи блога</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns><c>true</c> - если заголовок записи был изменён</returns>
    public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
            throw new InvalidOperationException($"Пост с id {PostId} не найден");

        if (Equals(post.Title, Title))
            return false;

        post.Title = Title;
        await _PostsRepository.Update(post, Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary>Изменение тела записи блога</summary>
    /// <param name="PostId">Идентификатор записи блога</param>
    /// <param name="Body">Тело записи блога</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns><c>true</c> - если тело записи был изменён</returns>
    public async Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
    {
        if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
            throw new InvalidOperationException($"Пост с id {PostId} не найден");

        if (Equals(post.Body, Body))
            return false;

        post.Body = Body;
        await _PostsRepository.Update(post, Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary>Изменение категории записи блога</summary>
    /// <param name="PostId">Идентификатор записи блога</param>
    /// <param name="CategoryName">Название категории</param>
    /// <param name="Cancel">Отмена асинхронной операции</param>
    /// <returns>Присвоенная категория</returns>
    /// <exception cref="InvalidOperationException">В случае если запись блога с указанным идентификатором не найдена</exception>
    public async Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
    {
        if (CategoryName is null) throw new ArgumentNullException(nameof(CategoryName));

        if (await _PostsRepository.GetById(PostId, Cancel).ConfigureAwait(false) is not { } post)
            throw new InvalidOperationException($"Пост с id {PostId} не найден");

        if (await _CategoriesRepository.GetByName(CategoryName, Cancel) is not { } category)
            category = await _CategoriesRepository.Add(new() { Name = CategoryName }, Cancel);

        if (post.Category.Id == category.Id)
            return category;

        post.Category = (category.Id, category.Name);

        await _PostsRepository.Update(post, Cancel).ConfigureAwait(false);

        return category;
    }

    #endregion
}
