using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;

/// <summary>
/// Роль пользователя
/// </summary>
public interface IRole : INamedEntity<string>
{
    /// <summary>Список пользователей обладающих этой ролью</summary>
    public ICollection<IUser> Users { get; set; }
}
