using System.ComponentModel.DataAnnotations;

using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Domain.Base;

/// <summary>Именованная сущность</summary>
/// <typeparam name="TKey">Тип первичного ключа</typeparam>
public abstract class NamedEntityModel<TKey> : EntityModel<TKey>, INamedEntity<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>Имя</summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>Инициализация новой именованной сущности</summary>
    protected NamedEntityModel() { }

    /// <summary>Инициализация новой именованной сущности</summary>
    /// <param name="Name">Имя</param>
    protected NamedEntityModel(string Name) => this.Name = Name;

    /// <summary>Инициализация новой именованной сущности</summary>
    /// <param name="Id">Идентификатор</param>
    protected NamedEntityModel(TKey Id) : base(Id) { }

    /// <summary>Инициализация новой именованной сущности</summary>
    /// <param name="Id">Идентификатор</param><param name="Name">Имя</param>
    protected NamedEntityModel(TKey Id, string Name) : base(Id) => this.Name = Name;
}

/// <summary>Именованная сущность</summary>
public abstract class NamedEntity : NamedEntityModel<int>, INamedEntity
{
    /// <summary>Инициализация новой именованной сущности</summary>
    protected NamedEntity() { }

    /// <summary>Инициализация новой именованной сущности</summary>
    /// <param name="Name">Имя</param>
    protected NamedEntity(string Name) : base(Name) { }

    /// <summary>Инициализация новой именованной сущности</summary>
    /// <param name="Id">Идентификатор</param>
    protected NamedEntity(int Id) : base(Id) { }

    /// <summary>Инициализация новой именованной сущности</summary>
    /// <param name="Id">Идентификатор</param><param name="Name">Имя</param>
    protected NamedEntity(int Id, string Name) : base(Id, Name) { }
}
