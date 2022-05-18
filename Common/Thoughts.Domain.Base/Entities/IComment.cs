namespace Thoughts.Domain.Base.Entities
{
    public interface IComment<TKey> : INote<TKey>
    {
        public IEnumerable<IComment<TKey>> Answers { get; set; }
    }
    public interface IComment : IComment<int> { }
}
