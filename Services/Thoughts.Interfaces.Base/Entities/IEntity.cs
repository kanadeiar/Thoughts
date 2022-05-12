namespace Thoughts.Interfaces.Base.Entities;

/// <summary>Сущность</summary>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public interface IEntity<TKey>
{
    TKey Id { get; set; }
}

/// <summary>Сущность</summary>
public interface IEntity : IEntity<int> { }