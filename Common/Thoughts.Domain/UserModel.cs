using System.ComponentModel.DataAnnotations;

using Thoughts.Domain.Base;

namespace Thoughts.Domain;
/// <summary>
/// Пользователь (автор)
/// </summary>
public class UserModel : EntityModel
{
    /// <summary>Статус пользователя</summary>
    [Required]
    public StatusModel Status { get; set; } = null!;
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
    public string NikName { get; set; } = null!;

    /// <summary>Роли пользователя</summary>
    public ICollection<RoleModel> Roles { get; set; } = new HashSet<RoleModel>();

    public UserModel() { }

    public UserModel(string LastName, string FirstName, string Patronymic, DateTime Birthday, string NikName)
    {
        this.LastName = LastName;
        this.FirstName = FirstName;
        this.Patronymic = Patronymic;
        this.Birthday = Birthday;
        this.NikName = NikName;
    }

    public override string ToString() => $"[id:{Id}] {string.Join(' ', LastName, FirstName, Patronymic)}";
}
