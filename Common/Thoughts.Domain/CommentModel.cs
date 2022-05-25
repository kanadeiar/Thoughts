
using System.ComponentModel.DataAnnotations;


using Thoughts.Domain.Base;

namespace Thoughts.Domain;

/// <summary>
/// Комментарий
/// </summary>    
public class CommentModel : EntityModel
{
    /// <summary>Дата комментария</summary>
    public DateTime Date { get; set; }

    /// <summary>Запись к которой принадлежит комментарий</summary>
    [Required]
    public PostModel Post { get; set; } = null!;

    /// <summary>Автор комментария</summary>
    [Required]
    public UserModel User { get; set; } = null!;

    /// <summary>Текст комментария</summary>
    [Required]
    public string Body { get; set; } = null!;

    /// <summary>Родительский комментарий</summary>
    public CommentModel? ParentComment { get; set; } = null!;

    /// <summary>Список дочерних комментариев</summary>
    public ICollection<CommentModel> ChildrenComment { get; set; } = new HashSet<CommentModel>();

    /// <summary>Признак удалённой записи</summary>
    public bool IsDeleted { get; set; }

    public CommentModel() { }

    public CommentModel(DateTime date, PostModel post, UserModel user, CommentModel parentComment, string body, bool isdeleted)
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
