namespace Thoughts.Extensions.Base
{
    public interface IMapper<in TSource, out TResult>
    {
        TResult Map(TSource item);
    }
}
