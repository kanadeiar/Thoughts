namespace Thoughts.Domain.Base.Entities
{
    public interface IBlogNote<TKey> : INote<TKey>
    {
        public string Title { get; set; }
        public ICategory<TKey> Category { get; set; }
        public IEnumerable<ITag<TKey>> Tags { get; set; }
        public IEnumerable<IComment<TKey>> Comments { get; set; }
    }
    public interface IBlogNote : IBlogNote<int> { }
}
