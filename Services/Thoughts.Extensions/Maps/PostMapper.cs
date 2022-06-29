using Thoughts.Extensions.Base;
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

namespace Thoughts.Extensions.Maps
{
    public class PostMapper : IMapper<PostDal, PostDom>, IMapper<PostDom, PostDal>
    {
        public PostDom Map(PostDal item)
        {
            if (item is null) return default;

            var post = new PostDom()
            {
                Id = item.Id,
                Status = (StatusDom)item.Status,
                Date = item.Date,
                User = new UserDom
                {
                    Id = item.User.Id,
                },
                Title = item.Title,
                Body = item.Body,
                PublicationsDate = item.PublicationDate,
            };
            MapsCash.PostDomCash.Add(post);

            post.User = MapsHelper.FindUserOrMapNew(item.User);

            post.Category = MapsHelper.FindCategoryOrMapNew(item.Category);

            foreach (var tag in item.Tags)
                post.Tags.Add(MapsHelper.FindTagOrMapNew(tag));

            foreach (var comment in item.Comments)
                post.Comments.Add(MapsHelper.FindCommentOrMapNew(comment));

            return post;
        }

        public PostDal Map(PostDom item)
        {
            if(item is null) return default;

            var post = new PostDal()
            {
                Id = item.Id,
                Status = (StatusDal)item.Status,
                Date = item.Date,
                Title = item.Title,
                Body = item.Body,
                PublicationDate = item.PublicationsDate,
            };
            MapsCash.PostDalCash.Add(post);

            post.User = MapsHelper.FindUserOrMapNew(item.User);

            post.Category = MapsHelper.FindCategoryOrMapNew(item.Category);

            foreach (var tag in item.Tags)
                post.Tags.Add(MapsHelper.FindTagOrMapNew(tag));

            foreach (var comment in item.Comments)
                post.Comments.Add(MapsHelper.FindCommentOrMapNew(comment));

            return post;
        }
    }

    public class CategoryMapper : IMapper<CategoryDal, CategoryDom>, IMapper<CategoryDom, CategoryDal>
    {
        public CategoryDal Map(CategoryDom item)
        {
            if (item is null) return default;
            var cat = new CategoryDal()
            {
                Id = item.Id,
                Name = item.Name,
                Status = (StatusDal)item.Status,
            };
            MapsCash.CategoryDalCash.Add(cat);

            foreach (var post in item.Posts)
                cat.Posts.Add(MapsHelper.FindPostOrMapNew(post));

            return cat;
        }

        public CategoryDom Map(CategoryDal item)
        {
            if (item is null) return default;
            var cat = new CategoryDom()
            {
                Id = item.Id,
                Name = item.Name,
                Status = (StatusDom)item.Status,
            };
            MapsCash.CategoryDomCash.Add(cat);

            foreach (var post in item.Posts)
                cat.Posts.Add(MapsHelper.FindPostOrMapNew(post));

            return cat;
        }
    }

    public class CommentMapper : IMapper<CommentDal, CommentDom>, IMapper<CommentDom, CommentDal>
    {
        public CommentDal Map(CommentDom item)
        {
            if (item is null) return default;

            var com = new CommentDal()
            {
                Id = item.Id,
                Body = item.Body,
                IsDeleted = item.IsDeleted,
                Date = item.Date,
            };
            MapsCash.CommentDalCash.Add(com);

            com.Post = MapsHelper.FindPostOrMapNew(item.Post);

            com.ParentComment = MapsHelper.FindCommentOrMapNew(item.ParentComment);

            com.User = MapsHelper.FindUserOrMapNew(item.User);

            foreach (var comment in item.ChildrenComment)
                com.ChildrenComment.Add(MapsHelper.FindCommentOrMapNew(comment));

            return com;
        }

        public CommentDom Map(CommentDal item)
        {
            if (item is null) return default;

            var com = new CommentDom()
            {
                Id = item.Id,
                Body = item.Body,
                IsDeleted = item.IsDeleted,
                Date = item.Date,
            };
            MapsCash.CommentDomCash.Add(com);

            com.Post = MapsHelper.FindPostOrMapNew(item.Post);

            com.ParentComment = MapsHelper.FindCommentOrMapNew(item.ParentComment);

            com.User = MapsHelper.FindUserOrMapNew(item.User);

            foreach (var comment in item.ChildrenComment)
                com.ChildrenComment.Add(MapsHelper.FindCommentOrMapNew(comment));

            return com;
        }
    }

    public class RoleMapper : IMapper<RoleDal, RoleDom>, IMapper<RoleDom, RoleDal>
    {
        public RoleDom Map(RoleDal item)
        {
            if (item is null) return default;

            var role = new RoleDom()
            {
                Id = item.Id,
                Name = item.Name,
            };
            MapsCash.RoleDomCash.Add(role);

            foreach (var user in item.Users)
                role.Users.Add(MapsHelper.FindUserOrMapNew(user));

            return role;
        }

        public RoleDal Map(RoleDom item)
        {
            if (item is null) return default;

            var role = new RoleDal()
            {
                Id = item.Id,
                Name = item.Name,
            };
            MapsCash.RoleDalCash.Add(role);

            foreach (var user in item.Users)
                role.Users.Add(MapsHelper.FindUserOrMapNew(user));

            return role;
        }
    }

    public class TagMapper : IMapper<TagDal, TagDom>, IMapper<TagDom, TagDal>
    {
        public TagDal Map(TagDom item)
        {
            if (item is null) return default;

            var tag = new TagDal()
            {
                Id = item.Id,
                Name = item.Name,
            };
            MapsCash.TagDalCash.Add(tag);

            foreach (var post in item.Posts)
                tag.Posts.Add(MapsHelper.FindPostOrMapNew(post));

            return tag;
        }

        public TagDom Map(TagDal item)
        {
            if (item is null) return default;

            var tag = new TagDom()
            {
                Id = item.Id,
                Name = item.Name,
            };
            MapsCash.TagDomCash.Add(tag);

            foreach (var post in item.Posts)
                tag.Posts.Add(MapsHelper.FindPostOrMapNew(post));

            return tag;
        }
    }

    public class UserMapper : IMapper<UserDal, UserDom>, IMapper<UserDom, UserDal>
    {
        public UserDal Map(UserDom item)
        {
            if (item is null) return default;

            var user = new UserDal()
            {
                Id = item.Id,
                NickName = item.NickName,
                LastName = item.LastName,
                FirstName = item.FirstName,
                Patronymic = item.Patronymic,
                Birthday = item.Birthday,
                Status = (StatusDal)item.Status,
            };
            MapsCash.UserDalCash.Add(user);

            foreach (var role in item.Roles)
                user.Roles.Add(MapsHelper.FindRoleOrMapNew(role));

            return user;
        }

        public UserDom Map(UserDal item)
        {
            if (item is null) return default;

            var user = new UserDom()
            {
                Id = item.Id,
                NickName = item.NickName,
                LastName = item.LastName,
                FirstName = item.FirstName,
                Patronymic = item.Patronymic,
                Birthday = item.Birthday,
                Status = (StatusDom)item.Status,
            };
            MapsCash.UserDomCash.Add(user);

            foreach (var role in item.Roles)
                user.Roles.Add(MapsHelper.FindRoleOrMapNew(role));

            return user;
        }
    }
}
