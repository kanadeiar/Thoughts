using System.Diagnostics.CodeAnalysis;

namespace Thoughts.Interfaces.Base
{
    public interface IMapper<TSource, TResult>
    {
        [return: NotNullIfNotNull("item")]
        TResult? Map(TSource? item);

        //[return: NotNullIfNotNull("item")] // todo: Посмотреть варианты реализации
        //TSource? MapBack(TResult? item);
    }
}
