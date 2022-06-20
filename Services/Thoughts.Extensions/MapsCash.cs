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
        public static HashSet<PostDal> PostDalCash { get; } = new HashSet<PostDal>();
        public static HashSet<StatusDal> StatusDalCash { get; } = new HashSet<StatusDal>();
        public static HashSet<CategoryDal> CategoryDalCash { get; } = new HashSet<CategoryDal>();
        public static HashSet<CommentDal> CommentDalCash { get; } = new HashSet<CommentDal>();
        public static HashSet<RoleDal> RoleDalCash { get; } = new HashSet<RoleDal>();
        public static HashSet<TagDal> TagDalCash { get; } = new HashSet<TagDal>();
        public static HashSet<UserDal> UserDalCash { get; } = new HashSet<UserDal>();
        public static HashSet<PostDom> PostDomCash { get; } = new HashSet<PostDom>();
        public static HashSet<StatusDom> StatusDomCash { get; } = new HashSet<StatusDom>();
        public static HashSet<CategoryDom> CategoryDomCash { get; } = new HashSet<CategoryDom>();
        public static HashSet<CommentDom> CommentDomCash { get; } = new HashSet<CommentDom>();
        public static HashSet<RoleDom> RoleDomCash { get; } = new HashSet<RoleDom>();
        public static HashSet<TagDom> TagDomCash { get; } = new HashSet<TagDom>();
        public static HashSet<UserDom> UserDomCash { get; } = new HashSet<UserDom>();
    }
}
