using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>Пользователь (автор)</summary>
[Index(nameof(LastName), nameof(FirstName), nameof(Patronymic), IsUnique = true, Name = "NameIndex")]
public class User : Entity<string>
{
    /// <summary>Фамилия</summary>
    [Required, MinLength(2)]
    public string LastName { get; set; } = null!;

    /// <summary>Имя</summary>
    [Required, MinLength(2)]
    public string FirstName { get; set; } = null!;

    /// <summary>Отчество</summary>
    public string? Patronymic { get; set; }

    /// <summary>Дата рождения</summary>
    [Column(TypeName = "date")]
    public DateOnly? Birthday { get; set; }

    /// <summary>Псевдоним (отображаемое имя автора)</summary>
    [Required]
    public string NickName { get; set; } = null!;

    /// <summary>Статус пользователя</summary>
    [Required]
    public Status Status { get; set; }

    /// <summary>Id пользователя Identity</summary>
    [StringLength(100)]
    public string IdentityUserId { get; set; }

    /// <summary>Роли пользователя</summary>
    public ICollection<Role> Roles { get; set; } = new HashSet<Role>();

    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    public override string ToString() => $"[id:{Id}] {string.Join(' ', LastName, FirstName, Patronymic)}";
}
