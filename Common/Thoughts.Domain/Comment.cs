
using System.ComponentModel.DataAnnotations;


using Thoughts.Domain.Base;

namespace Thoughts.Domain;

/// <summary>
/// Комментарий
/// </summary>    
public class Comment:Entity
{
    /// <summary>Дата комментария</summary>
    public DateTime Date { get; set; }

    /// <summary>Запись к которой принадлежит комментарий</summary>
    [Required]
    public Post Post { get; set; } = null!;

    /// <summary>Автор комментария</summary>
    [Required] 
    public User User { get; set; }=null!;

    /// <summary>Текст комментария</summary>
    [Required] 
    public string Body { get; set; } = null!;

    /// <summary>Родительский комментарий</summary>
    public Comment? ParentComment { get; set; } = null!;

    /// <summary>Список дочерних комментариев</summary>
    public ICollection<Comment> ChildrenComment { get; set; } = new HashSet<Comment>();

    /// <summary>Признак удалённой записи</summary>
    public bool IsDeleted { get; set; }

    public Comment() { }

    public Comment(DateTime date, Post post, User user, Comment parentComment,string body, bool isdeleted ) 
    {
        Date = date;
        Post = post;
        User = user;
        Body = body;
        IsDeleted = isdeleted;
        ParentComment = parentComment;
    }

    public override string ToString() => $"{Date}, {User.NikName}: {Body}";

}
