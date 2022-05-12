using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.Entities.Base;
/// <summary>
/// Пользователь (автор)
/// </summary>

[Index(nameof(LastName), nameof(FirstName), nameof(Patronymic), IsUnique = true, Name = "NameIndex")]
public abstract class User : Entity
{
    /// <summary>Фамилия</summary>
    public string LastName { get; set; } = null!;

    /// <summary>Имя</summary>
    public string FirstName { get; set; } = null!;

    /// <summary>Отчество</summary>
    public string Patronymic { get; set; } = null!;

    /// <summary>Дата рождения</summary>
    public DateTime Birthday { get; set; }

    /// <summary>Псевдоним (отображаемое имя автора)</summary>
    public string NikName { get; set; } = null!;

    /// <summary>Роли пользователя</summary>
    public ICollection<Role> Roles { get; set; }=null!;

    protected User() { }

    protected User(string LastName, string FirstName, string Patronymic, DateTime Birthday, string NikName)
    {
        this.LastName = LastName;
        this.FirstName = FirstName;
        this.Patronymic = Patronymic;
        this.Birthday = Birthday;
        this.NikName = NikName;
    }

    public override string ToString() => $"[id:{Id}] {string.Join(' ', LastName, FirstName, Patronymic)}";
}
