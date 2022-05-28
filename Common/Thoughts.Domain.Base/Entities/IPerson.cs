using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base.Entities;

public interface IPerson<TKey> : IEntity<TKey>
{
    public string LastName { get; set; }
    
    public string FirstName { get; set; }
    
    public string Patronymic { get; set; }
}

public interface IPerson : IPerson<int>, IEntity { }

