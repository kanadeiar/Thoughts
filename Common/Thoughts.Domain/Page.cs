using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Domain
{
    public record Page<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount) : IPage<T>
    {
        public int ItemsCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
