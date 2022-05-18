using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

using Thoughts.DAL.Sqlite;
using Thoughts.Interfaces.Base.Entities;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.Repositories;

public abstract class BaseSqlRepository<T> : IRepository<T> where T : class, IEntity<int>
{
    private readonly ContextSqlite _dbContextSql;
    private readonly ILogger<BaseSqlRepository<T>> _logger;
    private readonly DbSet<T> _table;

    protected BaseSqlRepository(ContextSqlite dbContextSql, ILogger<BaseSqlRepository<T>> logger)
    {
        _dbContextSql = dbContextSql;
        _logger = logger;
        _table = dbContextSql.Set<T>();
    }

    #region Async
        public async Task<bool> ExistIdAsync(int Id, CancellationToken Cancel = default)
    {
        var result = await _table.FindAsync(Id, Cancel).ConfigureAwait(false);
        return result is not null; 
    }

    public virtual async Task<bool> ExistAsync(T item, CancellationToken cancel = default) => await ExistIdAsync(item.Id, cancel);

    public async Task<int> GetCountAsync(CancellationToken cancel = default) =>
        (await _table.ToListAsync(cancel).ConfigureAwait(false)).Count;
    
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default) => await _table.ToListAsync(cancel).ConfigureAwait(false);

    public async Task<T> GetByIdAsync(int id, CancellationToken Cancel = default) => 
        await _table.FindAsync(id).ConfigureAwait(false) ?? throw new InvalidOperationException("Объект с указанным идентификатором не был найден");

    public virtual async Task<T> AddAsync(T item, CancellationToken cancel = default)
    {
        var result = _table.Add(item).Entity;
        await SaveChangesAsync(cancel).ConfigureAwait(false);
        return result;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
    {
        _table.AddRange(items);
        await SaveChangesAsync(cancel).ConfigureAwait(false);
    }

    public virtual async Task<T> UpdateAsync(T item, CancellationToken cancel = default)
    {
        var result = _table.Update(item).Entity;
        await SaveChangesAsync(cancel).ConfigureAwait(false);
        return result;
    }

    public Task<T?> UpdateAsync(int id, Action<T> itemUpdated, CancellationToken cancel = default) => throw new NotImplementedException();

    public virtual async Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
    {
        _table.UpdateRange(items);
        await SaveChangesAsync(cancel).ConfigureAwait(false);
    }

    public virtual async Task<T> DeleteAsync(T item, CancellationToken cancel = default)
    {
        var result = _table.Remove(item).Entity;
        await SaveChangesAsync(cancel).ConfigureAwait(false);
        return result;
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
    {
        _table.RemoveRange(items);
        await SaveChangesAsync(cancel).ConfigureAwait(false);
    }

    public virtual async Task<T> DeleteByIdAsync(int id, CancellationToken cancel = default)
    {
        var entity = await GetByIdAsync(id, cancel).ConfigureAwait(false);
        _dbContextSql.Entry(entity).State = EntityState.Deleted;
        await SaveChangesAsync(cancel).ConfigureAwait(false);
        return entity;
    }

    public async Task<bool> IsEmptyAsync(CancellationToken cancel = default) => (await _table.ToListAsync(cancel).ConfigureAwait(false)).Any();

    public async Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default) => 
        await _table.Skip(skip).Take(count).ToListAsync(cancel).ConfigureAwait(false);

    public Task<IPage<T>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancel = default) => throw new NotImplementedException();

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancel = default)
    {
        try
        {
            return await _dbContextSql.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e, "Concurrency error happened");
            throw new Exception(e.Message);
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Something wrong happened while updating database");
            throw new Exception(e.Message);
        }
        catch (RetryLimitExceededException e)
        {
            _logger.LogError(e, "Something wrong happened while trying to connect to sql server");
            throw new Exception(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something wrong happened while executing SaveChanges");
            throw new Exception(e.Message);
        }
    }
    

    #endregion
    #region Sync

    public bool ExistId(int id) => ExistIdAsync(id).Result;

    public bool Exist(T item) => ExistAsync(item).Result;

    public int GetCount() => GetCountAsync().Result;

    public IEnumerable<T> GetAll() => GetAllAsync().Result;

    public T GetById(int id) => GetByIdAsync(id).Result;

    public T Add(T item) => AddAsync(item).Result;

    public void AddRange(IEnumerable<T> items) => AddRangeAsync(items).RunSynchronously();

    public T Update(T item) => UpdateAsync(item).Result;
    
    public T Update(int id, Action<T> itemUpdated) => throw new NotImplementedException();

    public void UpdateRange(IEnumerable<T> items) => UpdateRangeAsync(items).RunSynchronously();

    public T Delete(T item) => DeleteAsync(item).Result;
    
    public void DeleteRange(IEnumerable<T> items) => DeleteRangeAsync(items).RunSynchronously();

    public T DeleteById(int id) => DeleteByIdAsync(id).Result;
    
    public bool IsEmpty() => IsEmptyAsync().Result;

    public IEnumerable<T> Get(int skip, int count) => GetAsync(skip, count).Result;

    public IPage<T> GetPage(int pageNumber, int pageSize) => throw new NotImplementedException();

    public int SaveChanges() => SaveChangesAsync().Result;

    #endregion
}