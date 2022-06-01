using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Domain;
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.InSQL;

public class SqlBlogPostManager : IBlogPostManager
{
    private readonly ThoughtsDB db;
    private readonly ILogger logger;

    /// <summary>
    /// Конструктор сервиса
    /// </summary>
    /// <param name="Db"> База данных </param>
    /// <param name="Logger"> Логгер </param>
    public SqlBlogPostManager(ThoughtsDB Db, ILogger Logger)
    {
        db = Db;
        logger = Logger;
    }

    #region Get all posts

    /// <summary>
    /// Аолучить все посты
    /// </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает все посты </returns>
    public async Task<IEnumerable<IPost>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        var posts = await db.Posts.ToArrayAsync(Cancel).ConfigureAwait(false);
        return (IEnumerable<IPost>)posts;
    }

    /// <summary>
    /// Получить количество всех постов
    /// </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает количество постов </returns>
    public async Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default)
    {
        var count = await db.Posts.CountAsync(Cancel).ConfigureAwait(false);
        return count;
    }

    /// <summary>
    /// Получение постов для пагинации (выборка)
    /// </summary>
    /// <param name="Skip"> Пропуск количества заданного диапазона постов </param>
    /// <param name="Take"> Получение заданного диапазона постов</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Урезанное перечисление постов (для пагинации) </returns>
    public async Task<IEnumerable<IPost>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        if(Take == 0)
            return Enumerable.Empty<IPost>();

        var posts = await db.Posts
            .Skip(Skip)
            .Take(Take)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);

        return (IEnumerable<IPost>)posts;
    }

    /// <summary>
    /// Получение страницы постов
    /// </summary>
    /// <param name="PageIndex"> Номер страницы </param>
    /// <param name="PageSize"> Размер страницы </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Страница постов </returns>
    public async Task<IPage<IPost>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var totalCount = await db.Posts.CountAsync(Cancel).ConfigureAwait(false);

        if (PageSize == 0)
            return new Page<IPost>(Enumerable.Empty<IPost>(), PageIndex, PageSize, totalCount);

        var posts = await db.Posts
            .Skip(PageIndex * PageSize)
            .Take(PageSize)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);

        return new Page<IPost>((IEnumerable<IPost>)posts,PageIndex,PageSize,totalCount);
    }

    #endregion

    #region Get user posts

    /// <summary>
    /// Получение всех постов пользователя
    /// </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Все пользовательские посты </returns>
    public async Task<IEnumerable<IPost>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        var userPosts = await db.Posts
            .FirstOrDefaultAsync(p => p.UserId == int.Parse(UserId), Cancel)
            .ConfigureAwait(false);

        return (IEnumerable<IPost>)userPosts!;
    }

    /// <summary>
    /// Получение количества всех постов пользователя
    /// </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Количество всех постов пользователя </returns>
    public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        int count = await db.Posts
            .CountAsync(p => p.UserId == int.Parse(UserId), Cancel)
            .ConfigureAwait(false);
            
        return count;
    }

    /// <summary>
    /// Получение выборки постов для пагинации конкретного пользователя
    /// </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Skip"> Пропуск количества заданного диапазона постов </param>
    /// <param name="Take"> Получение заданного диапазона постов</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Выборка постов для пагинации пользователя </returns>
    public async Task<IEnumerable<IPost>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
    {
        if (Take == 0)
            return Enumerable.Empty<IPost>();

        var posts = await GetAllPostsByUserIdAsync(UserId, Cancel);
        var page = posts.Skip(Skip).Take(Take);

        return page;
    }

    /// <summary>
    /// Получение страницы постов пользователя
    /// </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="PageIndex"> Номер страницы </param>
    /// <param name="PageSize"> Размер страницы </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Страница постов пользователя </returns>
    public async Task<IPage<IPost>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var totalCount = await db.Posts.CountAsync(Cancel).ConfigureAwait(false);
        
        if (PageSize == 0)
            return new Page<IPost>(Enumerable.Empty<IPost>(), PageIndex, PageSize, totalCount);
        
        var posts = await GetAllPostsByUserIdAsync(UserId, Cancel);

        return new Page<IPost>(posts, PageIndex, PageSize, totalCount);
    }

    #endregion

    /// <summary>
    /// Получение поста по его Id
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Конкретный пост </returns>
    public async Task<IPost?> GetPostAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == Id, Cancel).ConfigureAwait(false);

        return post as IPost;
    }

    /// <summary>
    /// Создание поста
    /// </summary>
    /// <param name="Title"> Заголовок </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="UserId"> Id автора поста </param>
    /// <param name="Category"> Категория поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Созданный пост </returns>
    public async Task<IPost> CreatePostAsync(
        string Title,
        string Body,
        string UserId,
        string Category,
        CancellationToken Cancel = default)
    {
        var post = new Post { Title = Title,
                              Body = Body,
                              CategoryName = Category, 
                              UserId = int.Parse(UserId) };

        await db.Posts.AddAsync(post).ConfigureAwait(false);
        await db.SaveChangesAsync(Cancel);

        return (IPost)post;
    }

    /// <summary>
    /// Удаление поста
    /// </summary>
    /// <param name="Id"> Идентификатор поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления </returns>
    public async Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
    {
        var dbPost = await GetPostAsync(Id, Cancel);
        
        if(dbPost is null)
            return false;

        db.Remove(dbPost);
        await db.SaveChangesAsync(Cancel);

        return true;
    }

    #region Tags

    /// <summary>
    /// Добавление тэга к посту
    /// </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата добавления тэга </returns>
    public async Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null || Tag == null)
            return false;

        var assignedTags = post.Tags;

        if(assignedTags != null && assignedTags.Any(n => n.Name == Tag)) 
            return true;

        var newTag = new Tag() { Name = Tag };

        post.Tags.Add((ITag)newTag);

        await db.SaveChangesAsync(Cancel);

        return true;
    }

    /// <summary>
    /// Удаление тэга из поста
    /// </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления тэга </returns>
    public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null || Tag == null)
            return false;

        var assignedTags = post.Tags;

        if (assignedTags is null || !assignedTags.Any(n => n.Name == Tag))
            return false;

        var removingTag = new Tag() { Name = Tag };

        assignedTags.Remove((ITag)removingTag);

        await db.SaveChangesAsync(Cancel);

        return false;
    }

    /// <summary>
    /// Получение тэгов поста
    /// </summary>
    /// <param name="Id"> Идентификатор тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление тэгов поста </returns>
    /// <exception cref="NotImplementedException"> Не найденный пост (?) </exception>
    public async Task<IEnumerable<ITag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(Id, Cancel);
        if (post == null) throw new NotImplementedException();

        var assignedTags = post.Tags;
        
        return assignedTags;
    }

    /// <summary>
    /// Получение всех постов по тэгу
    /// </summary>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление постов с конкретным тэгом </returns>
    public Task<IEnumerable<IPost>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        var searchingTag = new Tag() { Name = Tag };

        var searchingPosts = GetAllPostsAsync(Cancel)
            .Result
            .Where(t => t.Tags.Contains((ITag)searchingTag));

        return (Task<IEnumerable<IPost>>)searchingPosts;
    }

    #endregion

    #region Редактирование

    /// <summary>
    /// Изменение категории поста
    /// </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="CategoryName"> Название категории </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат категории поста </returns>
    /// <exception cref="NotImplementedException"> Не найденный пост </exception>
    public async Task<ICategory> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null) { throw new NotImplementedException(); }

        post.Category.Name = CategoryName;
        await db.SaveChangesAsync(Cancel);

        return post.Category;
    }

    /// <summary>
    /// Изменение заголовка поста
    /// </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Title"> Заголовок поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения заголовка поста</returns>
    public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);
        
        if(post == null)
            return false;
        
        post.Title = Title;
        await db.SaveChangesAsync(Cancel);
        
        return true;
    }

    /// <summary>
    /// Изменение тела поста
    /// </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения тела поста</returns>
    public async Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null)
            return false;

        post.Body = Body;
        await db.SaveChangesAsync(Cancel);

        return true;
    }

    /// <summary>
    /// Изменение статуса поста
    /// </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Status"> Текст статуса </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат статуса поста </returns>
    /// <exception cref="NotImplementedException"> Не найденный пост </exception>
    public async Task<IStatus> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post == null) { throw new NotImplementedException(); }

        post.Status.Name = Status;
        await db.SaveChangesAsync(Cancel);

        return post.Status;
    }

    #endregion
}
