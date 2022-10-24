using Thoughts.Interfaces.Base;
using Thoughts.Interfaces.Base.Entities;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.Mapping;

public class MappingRepository<TSource, TDestination> : IRepository<TDestination>
    where TDestination : class, IEntity<int> 
    where TSource : class, IEntity<int>
{
    private readonly IRepository<TSource> _SourceRepository;
    private readonly IMapper<TSource, TDestination> _Mapper;
    private readonly IMapper<TDestination, TSource> _BackMapper;

    public MappingRepository(
        IRepository<TSource> SourceRepository, 
        IMapper<TSource, TDestination> Mapper, 
        IMapper<TDestination, TSource> BackMapper)
    {
        _SourceRepository = SourceRepository;
        _Mapper = Mapper;
        _BackMapper = BackMapper;
    }

    public async Task<bool> ExistId(int Id, CancellationToken Cancel = default) => await _SourceRepository.ExistId(Id, Cancel);

    public async Task<bool> Exist(TDestination item, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<int> GetCount(CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<IEnumerable<TDestination>> GetAll(CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<IEnumerable<TDestination>> Get(int Skip, int Count, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<IPage<TDestination>> GetPage(int PageNumber, int PageSize, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<TDestination> GetById(int Id, CancellationToken Cancel = default)
    {
        var source_item = await _SourceRepository.GetById(Id, Cancel).ConfigureAwait(false);
        if (source_item is null) return null;

        var result_items = _Mapper.Map(source_item);
        return result_items;
    }

    public async Task<TDestination> Add(TDestination item, CancellationToken Cancel = default)
    {
        var destination_item = _BackMapper.Map(item);

        var result = await _SourceRepository.Add(destination_item, Cancel).ConfigureAwait(false);

        var result_item = _Mapper.Map(result);
        return result_item;
    }

    public async Task AddRange(IEnumerable<TDestination> items, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<TDestination> Update(TDestination item, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task UpdateRange(IEnumerable<TDestination> items, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<TDestination> Delete(TDestination item, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task DeleteRange(IEnumerable<TDestination> items, CancellationToken Cancel = default) => throw new NotImplementedException();

    public async Task<TDestination> DeleteById(int id, CancellationToken Cancel = default)
    {
        var result_item = await _SourceRepository.DeleteById(id, Cancel).ConfigureAwait(false);
        var result = _Mapper.Map(result_item);
        return result;
    }
}
