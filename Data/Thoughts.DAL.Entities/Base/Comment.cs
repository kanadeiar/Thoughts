
using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.Entities.Base
{
    /// <summary>
    /// Комментарий
    /// </summary>    
    public class Comment:Entity
    {
        /// <summary>Дата комментария</summary>
        public DateTime Date { get; set; }

        /// <summary>Запись к которой принадлежит комментарий</summary>
        public Post Post { get; set; } = null!;

        /// <summary>Автор комментария</summary>
        public User User { get; set; }=null!;

        /// <summary>Текст комментария</summary>
        public string Body { get; set; } = null!;

        /// <summary>Родительский комментарий</summary>
        public Comment ParentComment { get; set; } = null!;

        /// <summary>Список дочерних комментариев</summary>
        public ICollection<Comment> ChildrenComment { get; set; }=null!;

        /// <summary>Признак удалённой записи</summary>
        public bool IsDeleted { get; set; }=false;

        protected Comment() { }

        protected Comment(DateTime date, Post post, User user, Comment parentComment,string body, bool isdeleted ) 
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
}
