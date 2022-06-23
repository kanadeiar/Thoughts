using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.Domain;
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

using Post = Thoughts.Domain.Base.Entities.Post;
using Tag = Thoughts.Domain.Base.Entities.Tag;
using Category = Thoughts.Domain.Base.Entities.Category;

using Thoughts.Services.Mapping;

namespace Thoughts.Services.InSQL;

public class SqlBlogPostManager : IBlogPostManager
{
    private readonly ThoughtsDB _DB;
    private readonly ILogger<SqlBlogPostManager> _Logger;

    /// <summary> Конструктор сервиса </summary>
    /// <param name="Db"> База данных </param>
    /// <param name="Logger"> Логгер </param>
    public SqlBlogPostManager(ThoughtsDB Db, ILogger<SqlBlogPostManager> Logger)
    {
        _DB = Db;
        _Logger = Logger;
    }

    #region Get all posts

    /// <summary> Получить все посты </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает все посты </returns>
    public Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        var db_posts = _DB.Posts
            .Include(x => x.User);

        var domain_posts = db_posts.PostToDomain();

        return Task.FromResult(domain_posts)!;
    }

    /// <summary> Получить количество всех постов </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает количество постов </returns>
    public async Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default)
    {
        var count = await _DB.Posts.CountAsync(Cancel).ConfigureAwait(false);
        return count;
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

        var db_posts = await _DB.Posts
            .Include(x => x.User)
            .Skip(Skip)
            .Take(Take)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);

        var domain_posts = db_posts.PostToDomain();

        return domain_posts!;
    }

    /// <summary> Получение страницы постов </summary>
    /// <param name="PageIndex"> Номер страницы </param>
    /// <param name="PageSize"> Размер страницы </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Страница постов </returns>
    public async Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var total_count = await _DB.Posts.CountAsync(Cancel).ConfigureAwait(false);

        if (PageSize == 0)
            return new Page<Post>(Enumerable.Empty<Post>(), PageIndex, PageSize, total_count);

        var db_posts = await GetAllPostsAsync(Cancel).ConfigureAwait(false);

        return new Page<Post>(db_posts, PageIndex, PageSize, total_count);
    }

    #endregion

    #region Get user posts

    /// <summary> Получение всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Все пользовательские посты </returns>
    public Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        var db_posts = _DB.Posts.Where(p => p.User.Id == UserId);

        var domain_posts = db_posts.PostToDomain();

        return Task.FromResult(domain_posts)!;
    }

    /// <summary> Получение количества всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Количество всех постов пользователя </returns>
    public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        var count = await _DB.Posts
            .CountAsync(p => p.User.Id == UserId, Cancel)
            .ConfigureAwait(false);

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

        var posts = await GetAllPostsByUserIdAsync(UserId, Cancel);
        var page = posts.Skip(Skip).Take(Take);

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
        var total_count = await GetUserPostsCountAsync(UserId, Cancel).ConfigureAwait(false);

        if (PageSize == 0)
            return new Page<Post>(Enumerable.Empty<Post>(), PageIndex, PageSize, total_count);

        var posts = await GetAllPostsByUserIdAsync(UserId, Cancel);

        return new Page<Post>(posts, PageIndex, PageSize, total_count);
    }

    #endregion

    #region Get Create Delete

    /// <summary> Получение поста по его Id </summary>
    /// <param name="Id"></param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Конкретный пост </returns>
    public async Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default)
    {
        var db_post = await _DB.Posts
           .Include(post => post.Category)
           .Include(post => post.Tags)
           .FirstOrDefaultAsync(p => p.Id == Id, Cancel)
           .ConfigureAwait(false);

        if (db_post is null) return null;

        var domain_post = db_post.PostToDomain();

        return domain_post;
    }

    /// <summary> Создание поста </summary>
    /// <param name="Title"> Заголовок </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="UserId"> Id автора поста </param>
    /// <param name="Category"> Категория поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Созданный пост </returns>
    public async Task<Post> CreatePostAsync(
        string Title,
        string Body,
        string UserId,
        string Category,
        CancellationToken Cancel = default)
    {
        if (Title == null || Body == null || UserId == null || Category == null) throw new InvalidOperationException();

        var db_category = await _DB.Categories.FirstOrDefaultAsync(c => c.Name == Category, Cancel).ConfigureAwait(false);

        if (db_category is null) db_category = new DAL.Entities.Category { Name = Category };

        var post = new Post
        {
            Title = Title,
            Body = Body,
            Category = db_category.CategoryToDomain()!,
            User = _DB.Users.FirstOrDefault(user => user.Id == UserId).UserToDomain()!,
        };

        await _DB.Posts.AddAsync(post.PostToDAL()!, Cancel).ConfigureAwait(false);
        await _DB.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return post;
    }

    /// <summary> Удаление поста </summary>
    /// <param name="Id"> Идентификатор поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления </returns>
    public async Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
    {
        var db_post = await GetPostAsync(Id, Cancel);

        if (db_post is null)
            return false;

        _DB.Remove(db_post);
        await _DB.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return true;
    }

    #endregion

    #region Tags

    /// <summary>Добавление тэга к посту</summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата добавления тэга </returns>
    public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        var post = await _DB.Posts
           .Select(p => new DAL.Entities.Post { Id = p.Id })
           .FirstOrDefaultAsync(p => p.Id == PostId, Cancel).ConfigureAwait(false);

        if (post is null) return false;

        var tag = await _DB.Tags
           .Include(t => t.Posts)
           .FirstOrDefaultAsync(t => t.Name == Tag, Cancel);

        if (tag is null)
        {
            tag = new DAL.Entities.Tag { Name = Tag };
            await _DB.AddAsync(tag, Cancel);
        }

        post.Tags.Add(tag);

        await _DB.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary> Удаление тэга из поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления тэга </returns>
    public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        var post = await _DB.Posts
           .Select(p => new DAL.Entities.Post { Id = p.Id })
           .FirstOrDefaultAsync(p => p.Id == PostId, Cancel).ConfigureAwait(false);
        
        if (post is null || post.Tags.Count == 0) return false;
        
        var tag = await _DB.Tags
           .Include(t => t.Posts)
           .FirstOrDefaultAsync(t => t.Name == Tag, Cancel);

        if (tag is null) return false;

        post.Tags.Remove(tag);    // тут всё же не уверен, что удалится тег из поста, а не вообще тег

        await _DB.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary> Получение тэгов поста </summary>
    /// <param name="Id"> Идентификатор тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление тэгов поста </returns>
    /// <exception cref="InvalidOperationException"> Не найденный пост (?) </exception>
    public async Task<IEnumerable<Tag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(Id, Cancel);
        if (post is null)
            throw new InvalidOperationException($"Не найдена запись блога с идентификатором {Id}");

        return post.Tags;
    }

    /// <summary> Получение всех постов по тэгу </summary>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление постов с конкретным тэгом </returns>
    public async Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        var tag = await _DB.Tags
           .Include(t => t.Posts)
           .FirstOrDefaultAsync(tag => tag.Name == Tag, Cancel).ConfigureAwait(false);

        if (tag is null)
            return Enumerable.Empty<Post>();

        return tag.Posts.PostToDomain()!;
    }

    #endregion

    #region Редактирование

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

        var new_category = new Category { Name = CategoryName };

        post.Category = new_category;   // - тут всё же не уверен

        await _DB.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return post.Category!;
    }

    /// <summary> Изменение заголовка поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Title"> Заголовок поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения заголовка поста</returns>
    public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post is null) return false;

        post.Title = Title;
        await _DB.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return true;
    }

    /// <summary> Изменение тела поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения тела поста</returns>
    public async Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post is null)
            return false;

        post.Body = Body;
        await _DB.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return true;
    }

    #endregion
}
