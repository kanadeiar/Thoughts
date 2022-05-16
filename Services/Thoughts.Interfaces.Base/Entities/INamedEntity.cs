using System.ComponentModel.DataAnnotations;

namespace Thoughts.Interfaces.Base.Entities;

/// <summary>Именованная сущность</summary>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public interface INamedEntity<TKey> : IEntity<TKey>
{
    /// <summary>Имя</summary>
    [Required]
    string Name { get; set; }
}

/// <summary>Именованная сущность</summary>
public interface INamedEntity : INamedEntity<int>, IEntity { }