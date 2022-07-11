﻿using System.ComponentModel.DataAnnotations;

namespace Thoughts.Domain.Base.Entities;

/// <summary>Комментарий</summary>
public class Comment : EntityModel
{
    /// <summary>Дата комментария</summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>Запись к которой принадлежит комментарий</summary>
    [Required]
    public int PostId { get; set; }

    /// <summary>Автор комментария</summary>
    [Required]
    public User User { get; set; } = null!;

    /// <summary>Текст комментария</summary>
    [Required]
    public string Body { get; set; } = null!;

    /// <summary>Родительский комментарий</summary>
    public Comment? ParentComment { get; set; }

    /// <summary>Список дочерних комментариев</summary>
    public ICollection<int> ChildrenComment { get; set; } = new HashSet<int>();

    /// <summary>Признак удалённой записи</summary>
    public bool IsDeleted { get; set; }

    public override string ToString() => $"{Date}, {User.NickName}: {Body}";

}
