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
        if(Id == null) throw new ArgumentNullException(nameof(Id));

        var query = Items;

        var item = await Items.SingleOrDefaultAsync(p => p.Id!.Equals(Id));

        if(item == null) return false;

        return true;
    }

    public async Task<bool> Exist(T item, CancellationToken Cancel = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        var query = Items;

        var new_item = await Items.SingleOrDefaultAsync(p => p.Equals(item));

        if (new_item is null) return false;

        return true;
    }

    public async Task<int> GetCount(CancellationToken Cancel = default)
    {
        var query = Items;

        if (query is null) return 0;

        var result = await query.CountAsync();

        return result;
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
        if (Id is null) throw new ArgumentNullException(nameof(Id));

        var query = Items;

        //if (query.Count() == 0) throw new InvalidOperationException(nameof(query));

        var item = await query.SingleOrDefaultAsync(p => p.Id!.Equals(Id));

        return item!;
    }

    public async Task<T> Add(T item, CancellationToken Cancel = default)
    {
        if(item is null) throw new ArgumentNullException(nameof(item));

        await _db.AddAsync(item);
        await _db.SaveChangesAsync();

        return item;
    }

    public async Task AddRange(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));

        await _db.AddRangeAsync(items);
        await _db.SaveChangesAsync();       
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
        if (items is null) throw new ArgumentNullException(nameof(items));

        _db.UpdateRange(items);
        await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
    }

    public async Task<T> Delete(T item, CancellationToken Cancel = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        _db.Remove(item);
        await _db.SaveChangesAsync();

        return item;
    }

    public async Task DeleteRange(IEnumerable<T> items, CancellationToken Cancel = default)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));

        _db.RemoveRange(items);
        await _db.SaveChangesAsync();
    }

    public async Task<T> DeleteById(TKey id, CancellationToken Cancel = default)
    {
        if(id is null) throw new ArgumentNullException(nameof(id));

        var item = await GetById(id, Cancel).ConfigureAwait(false);

        _db.Remove(item);
        await _db.SaveChangesAsync();

        return item;
    }
}

public class DbRepository<T> : DbRepository<T, int>, IRepository<T> where T : class, IEntity<int>
{
    public DbRepository(ThoughtsDB db, ILogger<DbRepository<T>> Logger) : base(db, Logger) { }
}
