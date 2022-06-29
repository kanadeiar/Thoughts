using System.Diagnostics.CodeAnalysis;

namespace Thoughts.Interfaces.Base
{
    public interface IMapper<in TSource, out TResult>
    {
        [return: NotNullIfNotNull("item")]
        TResult? Map(TSource? item);
    }
}
