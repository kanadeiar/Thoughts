using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;

/// <summary>
/// Роль пользователя
/// </summary>
/// <typeparam name="TKye"></typeparam>
public interface IRole<TKye>:INamedEntity<TKye>
{
    /// <summary>Список пользователей обладающих этой ролью</summary>
    public ICollection<IUser<TKye>> Users { get; set; }
}
