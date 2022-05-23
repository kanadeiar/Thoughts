namespace Thoughts.Interfaces.Base.Entities;

public interface ITag<TKye>:INamedEntity<TKye>
{
    public ICollection<IPost<TKye>> Posts { get; set; }
}
