using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

using Thoughts.Domain;
using Thoughts.Domain.Base.Entities;

namespace Thoughts.Services;

public class RepositoryBlogPostManager : IBlogPostManager
{
    private readonly IRepository<Post> _postRepo;
    private readonly INamedRepository<Tag> _tagRepo;
    private readonly INamedRepository<Category> _categoryRepo;
    private readonly IRepository<User, string> _userRepo;

    public RepositoryBlogPostManager(
        IRepository<Post> PostRepo,
        INamedRepository<Tag> TagRepo,
        INamedRepository<Category> CategoryRepo,
        IRepository<User, string> UserRepo)
    {
        _postRepo = PostRepo;
        _tagRepo = TagRepo;
        _categoryRepo = CategoryRepo;
        _userRepo = UserRepo;
    }

    #region Get All Posts

    /// <summary> Получить все посты </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает все посты </returns>
    public async Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        var posts = await _postRepo.GetAll(Cancel).ConfigureAwait(false);
        return posts;
    }

    /// <summary> Получить количество всех постов </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает количество постов </returns>
    public async Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default)
    {
        var posts_count = await _postRepo.GetCount(Cancel).ConfigureAwait(false);
        return posts_count;
    }

    /// <summary> Получение постов для пагинации (выборка) </summary>
    /// <param name="Skip"> Пропуск количества заданного диапазона постов </param>
    /// <param name="Take"> Получение заданного диапазона постов</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Урезанное перечисление постов (для пагинации) </returns>
    public async Task<IEnumerable<Post>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        if (Take == 0)
            return Enumerable.Empty<Post>();

        var page = await _postRepo.Get(Skip, Take, Cancel).ConfigureAwait(false);

        return page;
    }

    /// <summary> Получение страницы постов </summary>
    /// <param name="PageIndex"> Номер страницы </param>
    /// <param name="PageSize"> Размер страницы </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Страница постов </returns>
    public async Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var total_count = await _postRepo.GetCount(Cancel).ConfigureAwait(false);

        //var total_count = await GetAllPostsCountAsync(Cancel).ConfigureAwait(false);

        if(PageSize == 0)
            return new Page<Post>(Enumerable.Empty<Post>(), PageIndex, PageSize, total_count);

        var page = await _postRepo.GetPage(PageIndex, PageSize, Cancel).ConfigureAwait(false);

        //здесь не уверен, всё же в конструкторе страницы обязательно указывать общее количество, а в интерфейсе репозитория общее количество не указывается
        return page;
    }

    #endregion

    #region Get All Posts By User

    /// <summary> Получение всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Все пользовательские посты </returns>
    public async Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        var all_posts = await _postRepo.GetAll(Cancel);

        var user_posts = all_posts.Where(p => p.User.Id == UserId);
        return user_posts;
    }

    /// <summary> Получение количества всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Количество всех постов пользователя </returns>
    public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        var all_posts = await _postRepo.GetAll(Cancel);

        var count = all_posts.Count(p => p.User.Id == UserId); // todo: надо обучить репозиторий выдавать записи по id указанного пользователя
        
        return count;
    }

    /// <summary> Получение выборки постов для пагинации конкретного пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Skip"> Пропуск количества заданного диапазона постов </param>
    /// <param name="Take"> Получение заданного диапазона постов</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Выборка постов для пагинации пользователя </returns>
    public async Task<IEnumerable<Post>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
    {
        if (Take == 0)
            return Enumerable.Empty<Post>();

        var all_posts_by_user_id = await GetAllPostsByUserIdAsync(UserId, Cancel);

        var page = all_posts_by_user_id.Skip(Skip).Take(Take);
        
        return page;
    }

    /// <summary> Получение страницы постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="PageIndex"> Номер страницы </param>
    /// <param name="PageSize"> Размер страницы </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Страница постов пользователя </returns>
    public async Task<IPage<Post>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var count = await GetUserPostsCountAsync(UserId,Cancel).ConfigureAwait(false);

        if (PageSize == 0)
            return new Page<Post>(Enumerable.Empty<Post>(), PageIndex, PageSize, count);

        //var user_posts = await GetAllPostsByUserIdAsync(UserId, Cancel).ConfigureAwait(false); //здесь не как в GetAllPostsPageAsync - метод GetPage из IRepository не даёт сделать выборку по Id
        var user_posts = await _postRepo.GetAll(Cancel).ConfigureAwait(false);

        var posts = user_posts.Where(p => p.User.Id == UserId).Skip(PageIndex * PageSize).Take(PageSize);

        return new Page<Post>(posts, PageIndex, PageSize, count);
    }

    #endregion

    #region Get Delete Create post

    /// <summary> Получение поста по его Id </summary>
    /// <param name="Id">Id поста</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Конкретный пост </returns>
    public async Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await _postRepo.GetById(Id, Cancel).ConfigureAwait(false);
        return post;
    }


    /// <summary> Удаление поста </summary>
    /// <param name="Id"> Идентификатор поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления </returns>
    public async Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(Id, Cancel).ConfigureAwait(false);
        
        if(post is null)
            return false;

        await _postRepo.DeleteById(Id, Cancel).ConfigureAwait(false);
        return true;
    }

    /// <summary> Создание поста </summary>
    /// <param name="Title"> Заголовок </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="UserId"> Id автора поста </param>
    /// <param name="Category"> Категория поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Созданный пост </returns>
    public async Task<Post> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default)
    {
        if (Title is null) throw new ArgumentNullException(nameof(Title));
        if (Body is null) throw new ArgumentNullException(nameof(Body));
        if (UserId is null) throw new ArgumentNullException(nameof(UserId));
        if (Category is null) throw new ArgumentNullException(nameof(Category));

        //if (string.IsNullOrEmpty(UserId)) throw new ArgumentException("Не указано значение идентификатора пользвоателя", nameof(UserId));
        //if (string.IsNullOrEmpty(Category)) throw new ArgumentException("Не указано значение категории", nameof(Category));

        if (UserId is not { Length: > 0 }) throw new ArgumentException("Не указан идентификатор пользователя", nameof(UserId));

        var user = await _userRepo.GetById(UserId, Cancel).ConfigureAwait(false);

        var post = new Post
        {
            Title = Title,
            Body = Body,
            User = user,
            //UserId = UserId, // <- тут да, схитрил, добавив внешний ключ для сущности юзера
            //Category = new_category,
            Category = await _categoryRepo.ExistName(Category, Cancel).ConfigureAwait(false)
                        ? await _categoryRepo.GetByName(Category, Cancel).ConfigureAwait(false)
                        : new Category { Name = Category },
        };

        //await _postRepo.Update(post, Cancel).ConfigureAwait(false);
        await _postRepo.Add(post,Cancel).ConfigureAwait(false);
        
        return post;

        //return await _postRepo.Add(post, Cancel).ConfigureAwait(false); // <- пришлось отказаться от такой реализации, так как в тесте не возвращался пост, хоть на Add настроен мок
    }

    #endregion

    #region Tag

    /// <summary>Добавление тэга к посту</summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата добавления тэга </returns>
    public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        if (Tag is null) throw new ArgumentNullException(nameof(Tag));

        var post = await GetPostAsync(PostId, Cancel).ConfigureAwait(false);
        
        if (post is null) return false;

        var tag = await _tagRepo.ExistName(Tag, Cancel).ConfigureAwait(false)
                ? await _tagRepo.GetByName(Tag, Cancel).ConfigureAwait(false)
                : new Tag { Name = Tag };

        if (post.Tags.Contains(tag))
            return true;              //  <- наверное всё же true, так как тег с таким именем уже есть в посте

        post.Tags.Add(tag);
        
        await _postRepo.Update(post, Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary> Удаление тэга из поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления тэга </returns>
    public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        if (Tag is null) throw new ArgumentNullException(nameof(Tag));

        var post = await GetPostAsync(PostId, Cancel).ConfigureAwait(false);
        var tag = await _tagRepo.GetByName(Tag, Cancel).ConfigureAwait(false);

        if (post is null || Tag is null) return false;

        if (!post.Tags.Contains(tag))
            return false;

        tag.Posts.Remove(post); // тут я подумал, а почему нет, раз есть в тегах есть ссылка на посты, которые связаны с этим тегом

        //post.Tags.Remove(tag);

        await _postRepo.Update(post, Cancel).ConfigureAwait(false);
        await _tagRepo.Update(tag, Cancel).ConfigureAwait(false); //нужно ли тут репозиторий тега обновлять

        return true;
    }

    /// <summary> Получение тэгов поста </summary>
    /// <param name="Id"> Идентификатор тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление тэгов поста </returns>
    /// <exception cref="InvalidOperationException"> Не найденный пост (?) </exception>
    public async Task<IEnumerable<Tag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(Id, Cancel).ConfigureAwait(false);

        return post is null ? Enumerable.Empty<Tag>() : post.Tags;
    }

    /// <summary> Получение всех постов по тэгу </summary>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление постов с конкретным тэгом </returns>
    public async Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        if (Tag is null) throw new ArgumentNullException(nameof(Tag));

        var tag = await _tagRepo.GetByName(Tag, Cancel);

        return tag is null ? Enumerable.Empty<Post>() : tag.Posts;
    }

    #endregion

    #region Edit

    /// <summary> Изменение заголовка поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Title"> Заголовок поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения заголовка поста</returns>
    public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel).ConfigureAwait(false);

        if (post is null || Title is null) return false;

        post.Title = Title;
        await _postRepo.Update(post, Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary> Изменение тела поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения тела поста</returns>
    public async Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel).ConfigureAwait(false);

        if (post is null || Body is null) return false;

        post.Body = Body;
        await _postRepo.Update(post, Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary> Изменение категории поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="CategoryName"> Название категории </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат категории поста </returns>
    /// <exception cref="InvalidOperationException"> Не найденный пост </exception>
    public async Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel).ConfigureAwait(false);
        
        if (post is null)
            throw new InvalidOperationException($"Не найдена запись блога с id:{PostId}");

        post.Category = await _categoryRepo.ExistName(CategoryName, Cancel).ConfigureAwait(false)
                      ? await _categoryRepo.GetByName(CategoryName, Cancel).ConfigureAwait(false)
                      : new Category { Name = CategoryName };

        await _postRepo.Update(post, Cancel).ConfigureAwait(false);
        await _categoryRepo.Update(post.Category, Cancel).ConfigureAwait(false);

        return post.Category;
    }

    #endregion
}
