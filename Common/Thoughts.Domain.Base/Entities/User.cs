using System.ComponentModel.DataAnnotations;

namespace Thoughts.Domain.Base.Entities;

/// <summary>Пользователь (автор)</summary>
public class User : EntityModel<string>
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
    public string? Patronymic { get; set; } = null!;

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
