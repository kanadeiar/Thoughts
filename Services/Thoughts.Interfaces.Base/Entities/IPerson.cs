namespace Thoughts.Interfaces.Base.Entities;

/// <summary>Персона</summary>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public interface IPerson<TKey> : IEntity<TKey>
{
    /// <summary>Фамилия</summary>
    public string LastName { get; set; }

    /// <summary>Имя</summary>
    public string FirstName { get; set; }

    /// <summary>Отчество</summary>
    public string Patronymic { get; set; }
}

/// <summary>Персона</summary>
public interface IPerson : IPerson<int> { }