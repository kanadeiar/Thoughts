using PostDal = Thoughts.DAL.Entities.Post;
using StatusDal = Thoughts.DAL.Entities.Status;
using CategoryDal = Thoughts.DAL.Entities.Category;
using CommentDal = Thoughts.DAL.Entities.Comment;
using RoleDal = Thoughts.DAL.Entities.Role;
using TagDal = Thoughts.DAL.Entities.Tag;
using UserDal = Thoughts.DAL.Entities.User;
using PostDom = Thoughts.Domain.Base.Entities.Post;
using StatusDom = Thoughts.Domain.Base.Entities.Status;
using CategoryDom = Thoughts.Domain.Base.Entities.Category;
using CommentDom = Thoughts.Domain.Base.Entities.Comment;
using RoleDom = Thoughts.Domain.Base.Entities.Role;
using TagDom = Thoughts.Domain.Base.Entities.Tag;
using UserDom = Thoughts.Domain.Base.Entities.User;

namespace Thoughts.Extensions
{
    public static class MapsCash
    {
        public static HashSet<PostDal> PostDalCash { get; } = new();
        public static HashSet<StatusDal> StatusDalCash { get; } = new();
        public static HashSet<CategoryDal> CategoryDalCash { get; } = new();
        public static HashSet<CommentDal> CommentDalCash { get; } = new();
        public static HashSet<RoleDal> RoleDalCash { get; } = new();
        public static HashSet<TagDal> TagDalCash { get; } = new();
        public static HashSet<UserDal> UserDalCash { get; } = new();
        public static HashSet<PostDom> PostDomCash { get; } = new();
        public static HashSet<StatusDom> StatusDomCash { get; } = new();
        public static HashSet<CategoryDom> CategoryDomCash { get; } = new();
        public static HashSet<CommentDom> CommentDomCash { get; } = new();
        public static HashSet<RoleDom> RoleDomCash { get; } = new();
        public static HashSet<TagDom> TagDomCash { get; } = new();
        public static HashSet<UserDom> UserDomCash { get; } = new();
    }
}
