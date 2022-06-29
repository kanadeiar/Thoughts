using System.ComponentModel.DataAnnotations;

namespace Thoughts.Domain.Base.Entities;

/// <summary>Пользователь (автор)</summary>
public class User : EntityModel<string>
{
    /// <summary>Статус пользователя</summary>
    [Required]
    public Status Status { get; set; }

    /// <summary>Фамилия</summary>
    [Required, MinLength(2)]
    public string LastName { get; set; } = null!;

    /// <summary>Имя</summary>
    [Required, MinLength(2)]
    public string FirstName { get; set; } = null!;

    /// <summary>Отчество</summary>
    public string? Patronymic { get; set; }

    /// <summary>Дата рождения</summary>
    public DateOnly? Birthday { get; set; }

    /// <summary>Псевдоним (отображаемое имя автора)</summary>
    [Required]
    public string NickName { get; set; } = null!;

    /// <summary>Роли пользователя</summary>
    public ICollection<Role> Roles { get; set; } = new HashSet<Role>();

    public override string ToString() => $"[id:{Id}] {string.Join(' ', LastName, FirstName, Patronymic)}";
}
