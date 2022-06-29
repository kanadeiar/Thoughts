namespace Thoughts.Extensions.Base
{
    public interface IMapper<T, R>
    {
        T Map(R item);
    }
}
