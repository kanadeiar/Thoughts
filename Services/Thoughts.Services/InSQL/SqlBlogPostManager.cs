using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.Domain;
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

using Post = Thoughts.Domain.Base.Entities.Post;
using Tag = Thoughts.Domain.Base.Entities.Tag;
using Category = Thoughts.Domain.Base.Entities.Category;
using Status = Thoughts.Domain.Base.Entities.Status;

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
    public async Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        //var db_posts = await _DB.Posts.ToArrayAsync(Cancel).ConfigureAwait(false);

        //var domain_posts = db_posts.Select(p => new Post
        //{
        //    Title = p.Title,
        //    Body = p.Body,
        //    // todo: либо настроить AutoMapper/Mapster, либо прописать методы-расширения для проекции DAL.Post -> Domain.Post
        //});

        //return domain_posts;

        var db_posts = _DB.Posts;

        var domain_posts = db_posts.Select(p => new Post
        {
            Title = p.Title,
            Body = p.Body,
            // todo: либо настроить AutoMapper/Mapster, либо прописать методы-расширения для проекции DAL.Post -> Domain.Post
        });

        return await domain_posts.ToArrayAsync(Cancel).ConfigureAwait(false);
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
            .Skip(Skip)
            .Take(Take)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);

        var domain_posts = db_posts.Select(p => new Post
        {
            Title = p.Title,
            Body = p.Body,
            // todo: либо настроить AutoMapper/Mapster, либо прописать методы-расширения для проекции DAL.Post -> Domain.Post
        });

        return domain_posts;
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

        var db_posts = await _DB.Posts
            .Skip(PageIndex * PageSize)
            .Take(PageSize)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);

        var domain_posts = db_posts.Select(p => new Post
        {
            Title = p.Title,
            Body = p.Body,
            // todo: либо настроить AutoMapper/Mapster, либо прописать методы-расширения для проекции DAL.Post -> Domain.Post
        });

        return new Page<Post>(domain_posts, PageIndex, PageSize, total_count);
    }

    #endregion

    #region Get user posts

    /// <summary> Получение всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Все пользовательские посты </returns>
    public async Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        var db_posts = _DB.Posts.Where(p => p.UserId == UserId);

        var domain_posts = db_posts.Select(p => new Post
        {
            Title = p.Title,
            Body = p.Body,
            // todo: либо настроить AutoMapper/Mapster, либо прописать методы-расширения для проекции DAL.Post -> Domain.Post
        });

        return await domain_posts.ToArrayAsync(Cancel).ConfigureAwait(false);
    }

    /// <summary> Получение количества всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Количество всех постов пользователя </returns>
    public async Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        var count = await _DB.Posts
            .CountAsync(p => p.UserId == UserId, Cancel)
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
        var total_count = await _DB.Posts.CountAsync(Cancel).ConfigureAwait(false);

        if (PageSize == 0)
            return new Page<Post>(Enumerable.Empty<Post>(), PageIndex, PageSize, total_count);

        var posts = await GetAllPostsByUserIdAsync(UserId, Cancel);

        return new Page<Post>(posts, PageIndex, PageSize, total_count);
    }

    #endregion

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

        var domain_post = new Post
        {
            Title = db_post.Title,
            Body = db_post.Body,
            // todo: либо настроить AutoMapper/Mapster, либо прописать методы-расширения для проекции DAL.Post -> Domain.Post
        };

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
        throw new NotImplementedException();

        //var post = new Post
        //{
        //    Title = Title,
        //    Body = Body,
        //    CategoryName = Category, // надо найти в БД запись с выбранной категорией (по её имени) и присвоить сюда значение
        //    UserId = UserId
        //};

        //await _DB.Posts.AddAsync(post, Cancel).ConfigureAwait(false);
        //await _DB.SaveChangesAsync(Cancel);

        //return (Post)post;
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
        await _DB.SaveChangesAsync(Cancel);

        return true;
    }

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
        if (post is null)
            return false;

        var tag = await _DB.Tags
           .Include(t => t.Posts)
           .FirstOrDefaultAsync(t => t.Name == Tag, Cancel);

        if (tag is null)
        {
            tag = new DAL.Entities.Tag { Name = Tag };
            await _DB.AddAsync(tag, Cancel);
        }

        tag.Posts.Add(post);

        //var post = await GetPostAsync(PostId, Cancel);

        //if (post is null || Tag is null)
        //    return false;

        //var assigned_tags = post.Tags;

        //if (assigned_tags.Any(n => n.Name == Tag))
        //    return true;

        //var new_tag = new Tag { Name = Tag };

        //post.Tags.Add((ITag)new_tag);

        await _DB.SaveChangesAsync(Cancel);

        return true;
    }

    /// <summary> Удаление тэга из поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления тэга </returns>
    public async Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();

        // todo: Переделать в соответствии с реализацией AssignTagAsync

        //var post = await GetPostAsync(PostId, Cancel);

        //if (post is null || Tag is null)
        //    return false;

        //var assigned_tags = post.Tags;

        //if (assigned_tags is null || !assigned_tags.Any(n => n.Name == Tag))
        //    return false;

        //var removing_tag = new Tag { Name = Tag };

        //assigned_tags.Remove((ITag)removing_tag);

        //await _DB.SaveChangesAsync(Cancel);

        //return false;
    }

    /// <summary> Получение тэгов поста </summary>
    /// <param name="Id"> Идентификатор тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление тэгов поста </returns>
    /// <exception cref="NotImplementedException"> Не найденный пост (?) </exception>
    public async Task<IEnumerable<Tag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(Id, Cancel);
        if (post is null)
            throw new InvalidOperationException($"Не найдена запись блога с идентификатором {Id}");

        var assigned_tags = post.Tags;

        return assigned_tags;
    }

    /// <summary> Получение всех постов по тэгу </summary>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление постов с конкретным тэгом </returns>
    public async Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        //var searching_tag = new Tag { Name = Tag };
        //var searching_posts = await GetAllPostsAsync(Cancel);
        //return searching_posts.Where(post => post.Tags.Select(tag => tag.Name).Contains(Tag));

        var tag = await _DB.Tags
           .Include(t => t.Posts)
           .FirstOrDefaultAsync(tag => tag.Name == Tag, Cancel).ConfigureAwait(false);

        if (tag is null)
            return Enumerable.Empty<Post>();

        return tag.Posts.Select(p => new Post
        {
            Title = p.Title,
            Body = p.Body,
            // todo: либо настроить AutoMapper/Mapster, либо прописать методы-расширения для проекции DAL.Post -> Domain.Post
        });
    }

    #endregion

    #region Редактирование

    /// <summary> Изменение категории поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="CategoryName"> Название категории </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат категории поста </returns>
    /// <exception cref="NotImplementedException"> Не найденный пост </exception>
    public async Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel).ConfigureAwait(false);

        if (post is null) 
            throw new InvalidOperationException($"Не найдена запись блога с id:{PostId}");

        throw new NotImplementedException("Переделать процесс редактирования категории");

        post.Category.Name = CategoryName; // todo: здесь произойдёт переименование категории у всех записей! А должна произойти смена категории у указанной записи!
        await _DB.SaveChangesAsync(Cancel);

        return post.Category;
    }

    /// <summary> Изменение заголовка поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Title"> Заголовок поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения заголовка поста</returns>
    public async Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post is null)
            return false;

        post.Title = Title;
        await _DB.SaveChangesAsync(Cancel);

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
        await _DB.SaveChangesAsync(Cancel);

        return true;
    }

    /// <summary> Изменение статуса поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Status"> Текст статуса </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат статуса поста </returns>
    /// <exception cref="NotImplementedException"> Не найденный пост </exception>
    public async Task<Status> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default)
    {
        var post = await GetPostAsync(PostId, Cancel);

        if (post is null)
            throw new InvalidOperationException($"Не найдена запись блога с id:{PostId}");

        throw new NotImplementedException("Переделать редактирование статуса");

        post.Status.Name = Status;
        await _DB.SaveChangesAsync(Cancel);

        return post.Status;
    }

    #endregion
}
