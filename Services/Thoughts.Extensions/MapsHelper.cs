using PostDal = Thoughts.DAL.Entities.Post;
using CategoryDal = Thoughts.DAL.Entities.Category;
using CommentDal = Thoughts.DAL.Entities.Comment;
using RoleDal = Thoughts.DAL.Entities.Role;
using TagDal = Thoughts.DAL.Entities.Tag;
using UserDal = Thoughts.DAL.Entities.User;
using PostDom = Thoughts.Domain.Base.Entities.Post;
using CategoryDom = Thoughts.Domain.Base.Entities.Category;
using CommentDom = Thoughts.Domain.Base.Entities.Comment;
using RoleDom = Thoughts.Domain.Base.Entities.Role;
using TagDom = Thoughts.Domain.Base.Entities.Tag;
using UserDom = Thoughts.Domain.Base.Entities.User;
using Thoughts.Extensions.Maps;

namespace Thoughts.Extensions
{
    public static class MapsHelper
    {
        public static UserDal FindUserOrMapNew(UserDom item)
        {
            var tmpUser = MapsCash.UserDalCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpUser is null)
            {
                tmpUser = new UserMapper().Map(item);
            }
            return tmpUser;
        }

        public static UserDom FindUserOrMapNew(UserDal item)
        {
            var tmpUser = MapsCash.UserDomCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpUser is null)
            {
                tmpUser = new UserMapper().Map(item);
            }
            return tmpUser;
        }

        public static CategoryDal FindCategoryOrMapNew(CategoryDom item)
        {
            var tmpCat = MapsCash.CategoryDalCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpCat is null)
            {
                tmpCat = new CategoryMapper().Map(item);
            }
            return tmpCat;
        }

        public static CategoryDom FindCategoryOrMapNew(CategoryDal item)
        {
            var tmpCat = MapsCash.CategoryDomCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpCat is null)
            {
                tmpCat = new CategoryMapper().Map(item);
            }
            return tmpCat;
        }

        public static TagDal FindTagOrMapNew(TagDom item)
        {
            var tmpTag = MapsCash.TagDalCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpTag is null)
            {
                tmpTag = new TagMapper().Map(item);
            }
            return tmpTag;
        }

        public static TagDom FindTagOrMapNew(TagDal item)
        {
            var tmpTag = MapsCash.TagDomCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpTag is null)
            {
                tmpTag = new TagMapper().Map(item);
            }
            return tmpTag;
        }

        public static CommentDal FindCommentOrMapNew(CommentDom item)
        {
            var tmpComment = MapsCash.CommentDalCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpComment is null)
            {
                tmpComment = new CommentMapper().Map(item);
            }

            return tmpComment;
        }

        public static CommentDom FindCommentOrMapNew(CommentDal item)
        {
            var tmpComment = MapsCash.CommentDomCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpComment is null)
            {
                tmpComment = new CommentMapper().Map(item);
            }

            return tmpComment;
        }

        public static PostDal FindPostOrMapNew(PostDom item)
        {
            var tmpPost = MapsCash.PostDalCash.FirstOrDefault(i => i.Id == item.Id);
            if (tmpPost is null)
            {
                tmpPost = new PostMapper().Map(item);
            }
            return tmpPost;
        }

        public static PostDom FindPostOrMapNew(PostDal item)
        {
            var tmpPost = MapsCash.PostDomCash.FirstOrDefault(i => i.Id == item.Id);
            if (tmpPost is null)
            {
                tmpPost = new PostMapper().Map(item);
            }
            return tmpPost;
        }

        public static RoleDal FindRoleOrMapNew(RoleDom item)
        {
            var tmpRole = MapsCash.RoleDalCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpRole is null)
            {
                tmpRole = new RoleMapper().Map(item);
            }
            return tmpRole;
        }

        public static RoleDom FindRoleOrMapNew(RoleDal item)
        {
            var tmpRole = MapsCash.RoleDomCash.FirstOrDefault(x => x.Id == item.Id);
            if (tmpRole is null)
            {
                tmpRole = new RoleMapper().Map(item);
            }
            return tmpRole;
        }
    }
}
