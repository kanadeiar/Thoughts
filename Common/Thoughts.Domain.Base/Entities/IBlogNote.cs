namespace Thoughts.Domain.Base.Entities
{
    public interface IBlogNote<TKey> : INote<TKey>
    {
        public string Title { get; set; }
        public ICategory Category { get; set; }
        public IEnumerable<ITag> Tags { get; set; }
    }
    public interface IBlogNote : IBlogNote<int> { }
}
