using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>
/// Пользователь (автор)
/// </summary>

[Index(nameof(LastName), nameof(FirstName), nameof(Patronymic), IsUnique = true, Name = "NameIndex")]
public class User : Entity<string>
{
    /// <summary>Статус пользователя</summary>
    [Required]
    public Status Status { get; set; } = null!;

    /// <summary>Фамилия</summary>
    [Required, MinLength(2)]
    public string LastName { get; set; } = null!;

    /// <summary>Имя</summary>
    [Required, MinLength(2)]
    public string FirstName { get; set; } = null!;

    /// <summary>Отчество</summary>
    public string? Patronymic { get; set; }

    /// <summary>Дата рождения</summary>
    public DateTime Birthday { get; set; }

    /// <summary>Псевдоним (отображаемое имя автора)</summary>
    [Required]
    public string NickName { get; set; } = null!;

    /// <summary>Роли пользователя</summary>
    public ICollection<Role> Roles { get; set; } = new HashSet<Role>();

    public User() { }

    public User(string LastName, string FirstName, string Patronymic, DateTime Birthday, string NickName)
    {
        this.LastName = LastName;
        this.FirstName = FirstName;
        this.Patronymic = Patronymic;
        this.Birthday = Birthday;
        this.NickName = NickName;
    }

    public override string ToString() => $"[id:{Id}] {string.Join(' ', LastName, FirstName, Patronymic)}";
}
