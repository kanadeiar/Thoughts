using System.Diagnostics.CodeAnalysis;

namespace Thoughts.Interfaces.Base
{
    public interface IMapper<TSource, TResult>
    {
        [return: NotNullIfNotNull("item")]
        TResult? Map(TSource? item);

        [return: NotNullIfNotNull("item")]
        TSource? Map(TResult? item);
    }
}
