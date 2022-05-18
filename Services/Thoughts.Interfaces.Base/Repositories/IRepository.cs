using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces.Base.Repositories;

/// <summary>Репозиторий сущностей</summary>
/// <typeparam name="T">Тип сущности, хранимой в репозитории</typeparam>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public interface IRepository<T, in TKey> where T : IEntity<TKey>
{
    /// <summary>Проверка репозитория на пустоту</summary>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Истина, если в репозитории нет ни одной сущности</returns>
    Task<bool> IsEmptyAsync(CancellationToken cancel = default);

    /// <summary>Существует ли сущность с указанным идентификатором</summary>
    /// <param name="id">Проверяемый идентификатор сущности</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Истина, если сущность с указанным идентификатором существует в репозитории</returns>
    Task<bool> ExistIdAsync(TKey id, CancellationToken cancel = default);

    /// <summary>Существует ли в репозитории указанная сущность</summary>
    /// <param name="item">Проверяемая сущность</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Истина, если указанная сущность существует в репозитории</returns>
    Task<bool> ExistAsync(T item, CancellationToken cancel = default);

    /// <summary>Получить число хранимых сущностей</summary>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    Task<int> GetCountAsync(CancellationToken cancel = default);

    /// <summary>Извлечь все сущности из репозитория</summary>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Перечисление всех сущностей репозитория</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default);

    /// <summary>Получить набор сущностей из репозитория в указанном количестве, предварительно пропустив некоторое количество</summary>
    /// <param name="skip">Число предварительно пропускаемых сущностей</param>
    /// <param name="count">Число извлекаемых из репозитория сущностей</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Перечисление полученных из репозитория сущностей</returns>
    Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default);

    /// <summary>Получить страницу с сущностями из репозитория</summary>
    /// <param name="pageNumber">Номер страницы начиная с нуля</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Страница с сущностями из репозитория</returns>
    Task<IPage<T>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancel = default);

    /// <summary>Получить сущность по указанному идентификатору</summary>
    /// <param name="id">Идентификатор извлекаемой сущности</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Сущность с указанным идентификатором в случае её наличия и null, если сущность отсутствует</returns>
    Task<T> GetByIdAsync(TKey id, CancellationToken cancel = default);

    /// <summary>Добавление сущности в репозиторий</summary>
    /// <param name="item">Добавляемая в репозиторий сущность</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Добавленная в репозиторий сущность</returns>
    Task<T> AddAsync(T item, CancellationToken cancel = default);

    /// <summary>Добавление перечисленных сущностей в репозиторий</summary>
    /// <param name="items">Перечисление добавляемых в репозиторий сущностей</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Задача, завершающаяся при завершении операции добавления сущностей</returns>
    Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);

    /// <summary>Добавление сущности в репозиторий с помощью фабричного метода</summary>
    /// <param name="itemFactory">Метод, формирующий добавляемую в репозиторий сущность</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Добавленная в репозиторий сущность</returns>
    Task<T> AddAsync(Func<T> itemFactory, CancellationToken cancel = default) => AddAsync(itemFactory(), cancel);

    /// <summary>Обновление сущности в репозитории</summary>
    /// <param name="item">Сущность, хранящая в себе информацию, которую надо обновить в репозитории</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Сущность из репозитория с обновлёнными данными</returns>
    Task<T> UpdateAsync(T item, CancellationToken cancel = default);

    /// <summary>Обновление сущности в репозитории</summary>
    /// <param name="id">Идентификатор обновляемой сущности</param>
    /// <param name="itemUpdated">Метод обновления информации в заданной сущности</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Сущность из репозитория с обновлёнными данными</returns>
    Task<T?> UpdateAsync(TKey id, Action<T> itemUpdated, CancellationToken cancel = default);

    /// <summary>Обновление перечисленных сущностей</summary>
    /// <param name="items">Перечисление сущностей, информацию из которых надо обновить в репозитории</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Задача, завершаемая при завершении операции обновления сущностей</returns>
    Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);

    /// <summary>Удаление сущности из репозитория</summary>
    /// <param name="item">Удаляемая из репозитория сущность</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Удалённая из репозитория сущность</returns>
    Task<T> DeleteAsync(T item, CancellationToken cancel = default);

    /// <summary>Удаление перечисления сущностей из репозитория</summary>
    /// <param name="items">Перечисление удаляемых сущностей</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Задача, завершаемая при завершении операции удаления сущностей</returns>
    Task DeleteRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);

    /// <summary>Удаление сущности по заданному идентификатору</summary>
    /// <param name="id">Идентификатор сущности, которую надо удалить</param>
    /// <param name="cancel">Признак отмены асинхронной операции</param>
    /// <returns>Удалённая из репозитория сущность</returns>
    Task<T> DeleteByIdAsync(TKey id, CancellationToken cancel = default);
    public Task<int> SaveChangesAsync(CancellationToken cancel);
}

/// <summary>Репозиторий сущностей</summary>
public interface IRepository<T> : IRepository<T, int> where T : IEntity<int> { }