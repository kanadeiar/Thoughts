using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

using Post = Thoughts.Domain.Base.Entities.Post;
using Tag = Thoughts.Domain.Base.Entities.Tag;
using Category = Thoughts.Domain.Base.Entities.Category;
using Status = Thoughts.Domain.Base.Entities.Status;
using User = Thoughts.Domain.Base.Entities.User;

namespace Thoughts.Services;

public class RepositoryBlogPostManager : IBlogPostManager
{
    private readonly IRepository<Post> _postRepo;
    private readonly IRepository<Tag> _tagRepo;
    private readonly IRepository<Category> _categoryRepo;
    private readonly IRepository<Status> _statusRepo;

    public RepositoryBlogPostManager(IRepository<Post> PostRepo,
                                     IRepository<Tag> TagRepo,
                                     IRepository<Category> CategoryRepo,
                                     IRepository<Status> StatusRepo)
    {
        _postRepo = PostRepo;
        _tagRepo = TagRepo;
        _categoryRepo = CategoryRepo;
        _statusRepo = StatusRepo;
    }

    #region Get All Posts

    /// <summary> Получить все посты </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает все посты </returns>
    public Task<IEnumerable<Post>> GetAllPostsAsync(CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получить количество всех постов </summary>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возвращает количество постов </returns>
    public Task<int> GetAllPostsCountAsync(CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получение постов для пагинации (выборка) </summary>
    /// <param name="Skip"> Пропуск количества заданного диапазона постов </param>
    /// <param name="Take"> Получение заданного диапазона постов</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Урезанное перечисление постов (для пагинации) </returns>
    public Task<IEnumerable<Post>> GetAllPostsSkipTakeAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получение страницы постов </summary>
    /// <param name="PageIndex"> Номер страницы </param>
    /// <param name="PageSize"> Размер страницы </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Страница постов </returns>
    public Task<IPage<Post>> GetAllPostsPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Get All Posts By User

    /// <summary> Получение всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Все пользовательские посты </returns>
    public Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(string UserId, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получение количества всех постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Количество всех постов пользователя </returns>
    public Task<int> GetUserPostsCountAsync(string UserId, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получение выборки постов для пагинации конкретного пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="Skip"> Пропуск количества заданного диапазона постов </param>
    /// <param name="Take"> Получение заданного диапазона постов</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Выборка постов для пагинации пользователя </returns>
    public Task<IEnumerable<Post>> GetAllPostsByUserIdSkipTakeAsync(string UserId, int Skip, int Take, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получение страницы постов пользователя </summary>
    /// <param name="UserId"> ID пользователя </param>
    /// <param name="PageIndex"> Номер страницы </param>
    /// <param name="PageSize"> Размер страницы </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Страница постов пользователя </returns>
    public Task<IPage<Post>> GetAllPostsByUserIdPageAsync(string UserId, int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Get Delete Create post

    /// <summary> Получение поста по его Id </summary>
    /// <param name="Id">Id поста</param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Конкретный пост </returns>
    public Task<Post?> GetPostAsync(int Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Создание поста </summary>
    /// <param name="Title"> Заголовок </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="UserId"> Id автора поста </param>
    /// <param name="Category"> Категория поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Созданный пост </returns>
    public Task<Post> CreatePostAsync(string Title, string Body, string UserId, string Category, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Удаление поста </summary>
    /// <param name="Id"> Идентификатор поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления </returns>
    public Task<bool> DeletePostAsync(int Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }


    #endregion

    #region Tag

    /// <summary>Добавление тэга к посту</summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата добавления тэга </returns>
    public Task<bool> AssignTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Удаление тэга из поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Флаг результата удаления тэга </returns>
    public Task<bool> RemoveTagAsync(int PostId, string Tag, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получение всех постов по тэгу </summary>
    /// <param name="Tag"> Текст тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление постов с конкретным тэгом </returns>
    public Task<IEnumerable<Post>> GetPostsByTag(string Tag, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Получение тэгов поста </summary>
    /// <param name="Id"> Идентификатор тэга </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Перечисление тэгов поста </returns>
    /// <exception cref="InvalidOperationException"> Не найденный пост (?) </exception>
    public Task<IEnumerable<Tag>> GetBlogTagsAsync(int Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Edit

    /// <summary> Изменение заголовка поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Title"> Заголовок поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения заголовка поста</returns>
    public Task<bool> ChangePostTitleAsync(int PostId, string Title, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Изменение тела поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Body"> Тело поста </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат флага результата изменения тела поста</returns>
    public Task<bool> ChangePostBodyAsync(int PostId, string Body, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Изменение статуса поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="Status"> Текст статуса </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат статуса поста </returns>
    /// <exception cref="NotImplementedException"> Не найденный пост </exception>
    public Task<Status> ChangePostStatusAsync(int PostId, string Status, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    /// <summary> Изменение категории поста </summary>
    /// <param name="PostId"> Идентификатор поста </param>
    /// <param name="CategoryName"> Название категории </param>
    /// <param name="Cancel"> Токен отмены </param>
    /// <returns> Возврат категории поста </returns>
    /// <exception cref="InvalidOperationException"> Не найденный пост </exception>
    public Task<Category> ChangePostCategoryAsync(int PostId, string CategoryName, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    #endregion
}
