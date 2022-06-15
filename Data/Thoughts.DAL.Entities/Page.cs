using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.DAL.Entities;

public record Page<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount) : IPage<T>
{
    public int ItemsCount => (int)Math.Ceiling((double)TotalCount / PageSize);
}
