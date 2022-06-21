using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Interfaces.Base.Entities;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.InSQL;

public class DbRepository<T, TKey> : IRepository<T, TKey> where T : class, IEntity<TKey>
{
    private readonly ThoughtsDB _db;
    private readonly ILogger<DbRepository<T, TKey>> _Logger;

    protected DbSet<T> Set { get; }

    public virtual IQueryable<T> Items => Set;

    public DbRepository(ThoughtsDB db, ILogger<DbRepository<T, TKey>> Logger)
    {
        _db = db;
        _Logger = Logger;

        Set = _db.Set<T>();
    }

    public async Task<bool> ExistId(TKey Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Exist(T item, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetCount(CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken Cancel = default)
    {
        var query = Items;

        if(query is null) return Enumerable.Empty<T>();

        if(query is not IOrderedQueryable<T>)
            query = query.OrderBy(item => item.Id);

        var result = await query.ToArrayAsync(Cancel).ConfigureAwait(false);
        
        return result;
    }

    public async Task<IEnumerable<T>> Get(int Skip, int Count, CancellationToken Cancel = default)
    {
        if (Skip < 0) throw new ArgumentOutOfRangeException(nameof(Skip), Skip, "Число пропускаемых элементов должно быть больше, либо равно 0");
        if (Count < 0) throw new ArgumentOutOfRangeException(nameof(Count), Count, "Число запрашиваемых элементов должно быть больше, либо равно 0");

        if (Count == 0) return Enumerable.Empty<T>();

        var query = Items;

        if (query is not IOrderedQueryable<T>)
            query = query.OrderBy(item => item.Id);

        if (Skip > 0)
            query = query.Skip(Skip);

        var result = await query.Take(Count).ToArrayAsync(Cancel).ConfigureAwait(false);

        return result;
    }

    public async Task<IPage<T>> GetPage(int PageNumber, int PageSize, CancellationToken Cancel = default)
    {
        if (PageNumber < 0) throw new ArgumentOutOfRangeException(nameof(PageNumber), PageNumber, "Номер страницы должен быть больше, либо равен 0");
        if (PageSize <= 0) throw new ArgumentOutOfRangeException(nameof(PageSize), PageSize, "Размер страницы должен быть больше 0");

        var query = Items;

        if (query is not IOrderedQueryable<T>)
            query = query.OrderBy(item => item.Id);

        var total_count = await query.CountAsync(Cancel).ConfigureAwait(false);

        if(PageNumber > 0)
            query = query.Skip(PageNumber * PageSize);

        query = query.Take(PageSize);

        var items = await query.ToArrayAsync(Cancel);

        var page = new Page<T>(items, PageNumber, PageSize, total_count);

        return page;
    }

    public async Task<T> GetById(TKey Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> Add(T item, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task AddRange(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> Update(T item, CancellationToken Cancel = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        _db.Update(item);

        await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);

        return item;
    }

    public async Task UpdateRange(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> Delete(T item, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteRange(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> DeleteById(TKey id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }
}

public class DbRepository<T> : DbRepository<T, int> where T : class, IEntity<int>
{
    public DbRepository(ThoughtsDB db, ILogger<DbRepository<T>> Logger) : base(db, Logger) { }
}
