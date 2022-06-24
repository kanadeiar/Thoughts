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
using Thoughts.Interfaces.Base.Entities;
using Thoughts.Domain.Base;

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
                UserId = item.User.Id,
                Title = item.Title,
                Body = item.Body,
                PublicationsDate = item.PublicationDate,
            };
            MapsCash.PostDomCash.Add(post);

            var tmpUser = MapsCash.UserDomCash.FirstOrDefault(x => x.Id == item.User.Id);
            if (tmpUser is null)
            {
                tmpUser = new UserMapper().Map(item.User);
            }
            post.User = tmpUser;

            var tmpCat = MapsCash.CategoryDomCash.FirstOrDefault(x => x.Id == item.Category.Id);
            if(tmpCat is null)
            {
                tmpCat = new CategoryMapper().Map(item.Category);
            }
            post.Category = tmpCat;

            foreach (var tag in item.Tags)
            {
                var tmpTag = MapsCash.TagDomCash.FirstOrDefault(x=> x.Id == tag.Id);
                if(tmpTag is null)
                {
                    tmpTag = new TagMapper().Map(tag);
                }
                post.Tags.Add(tmpTag);
            }

            foreach (var comment in item.Comments)
            {
                var tmpComment = MapsCash.CommentDomCash.FirstOrDefault(x => x.Id == comment.Id);
                if( tmpComment is null)
                {
                    tmpComment = new CommentMapper().Map(comment);
                }
                post.Comments.Add(tmpComment);
            }

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

            var tmpUser = MapsCash.UserDalCash.FirstOrDefault(x => x.Id == item.User.Id);
            if (tmpUser is null)
            {
                post.User = new UserMapper().Map(item.User);
            }
            post.User = tmpUser!;

            var tmpCat = MapsCash.CategoryDalCash.FirstOrDefault(x => x.Id == item.Category.Id);
            if (tmpCat is null)
            {
                post.Category = new CategoryMapper().Map(item.Category);
            }
            post.Category = tmpCat!;

            foreach (var tag in item.Tags)
            {
                var tmpTag = MapsCash.TagDalCash.FirstOrDefault(x => x.Id == tag.Id);
                if (tmpTag is null)
                {
                    tmpTag = new TagMapper().Map(tag);
                }
                post.Tags.Add(tmpTag);
            }

            foreach (var comment in item.Comments)
            {
                var tmpComment = MapsCash.CommentDalCash.FirstOrDefault(x => x.Id == comment.Id);
                if (tmpComment is null)
                {
                    tmpComment = new CommentMapper().Map(comment);
                }
                post.Comments.Add(tmpComment);
            }

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
            {
                var tmpPost = MapsCash.PostDalCash.FirstOrDefault(i => i.Id == post.Id);
                if(tmpPost is null)
                {
                    tmpPost = new PostMapper().Map(post);
                }
                cat.Posts.Add(tmpPost);
            }

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
            {
                var tmpPost = MapsCash.PostDomCash.FirstOrDefault(i => i.Id == post.Id);
                if (tmpPost is null)
                {
                    tmpPost = new PostMapper().Map(post);
                }
                cat.Posts.Add(tmpPost);
            }

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

            var tmpPost = MapsCash.PostDalCash.FirstOrDefault(x => x.Id == item.Post.Id);
            if(tmpPost is null)
            {
                tmpPost = new PostMapper().Map(item.Post);
            }
            com.Post = tmpPost;

            var tmpParentComment = MapsCash.CommentDalCash.FirstOrDefault(x => x.Id == item.ParentComment?.Id);
            if (tmpParentComment is null)
            {
                tmpParentComment = new CommentMapper().Map(item.ParentComment);
            }
            com.ParentComment = tmpParentComment;

            var tmpUser = MapsCash.UserDalCash.FirstOrDefault(x => x.Id == item.User.Id);
            if(tmpUser is null)
            {
                tmpUser = new UserMapper().Map(item.User);
            }
            com.User = tmpUser;

            foreach (var comment in item.ChildrenComment)
            {
                var tmpComment = MapsCash.CommentDalCash.FirstOrDefault(x => x.Id == comment.Id);
                if(tmpComment is null)
                {
                    tmpComment = new CommentMapper().Map(comment);
                }
                com.ChildrenComment.Add(tmpComment);
            }

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

            var tmpPost = MapsCash.PostDomCash.FirstOrDefault(x => x.Id == item.Post.Id);
            if (tmpPost is null)
            {
                tmpPost = new PostMapper().Map(item.Post);
            }
            com.Post = tmpPost;

            var tmpParentComment = MapsCash.CommentDomCash.FirstOrDefault(x => x.Id == item.ParentComment?.Id);
            if (tmpParentComment is null)
            {
                tmpParentComment = new CommentMapper().Map(item.ParentComment);
            }
            com.ParentComment = tmpParentComment;

            var tmpUser = MapsCash.UserDomCash.FirstOrDefault(x => x.Id == item.User.Id);
            if (tmpUser is null)
            {
                tmpUser = new UserMapper().Map(item.User);
            }
            com.User = tmpUser;

            foreach (var comment in item.ChildrenComment)
            {
                var tmpComment = MapsCash.CommentDomCash.FirstOrDefault(x => x.Id == comment.Id);
                if (tmpComment is null)
                {
                    tmpComment = new CommentMapper().Map(comment);
                }
                com.ChildrenComment.Add(tmpComment);
            }

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
            {
                var tmpUser = MapsCash.UserDomCash.FirstOrDefault(x => x.Id == user.Id);
                if (tmpUser is null)
                {
                    tmpUser = new UserMapper().Map(user);
                }
                role.Users.Add(tmpUser);
            }

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
            {
                var tmpUser = MapsCash.UserDalCash.FirstOrDefault(x => x.Id == user.Id);
                if (tmpUser is null)
                {
                    tmpUser = new UserMapper().Map(user);
                }
                role.Users.Add(tmpUser);
            }

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
            {
                var tmpPost = MapsCash.PostDalCash.FirstOrDefault(i => i.Id == post.Id);
                if (tmpPost is null)
                {
                    tmpPost = new PostMapper().Map(post);
                }
                tag.Posts.Add(tmpPost);
            }

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
            {
                var tmpPost = MapsCash.PostDomCash.FirstOrDefault(i => i.Id == post.Id);
                if (tmpPost is null)
                {
                    tmpPost = new PostMapper().Map(post);
                }
                tag.Posts.Add(tmpPost);
            }

            return tag;
        }
    }
    public class UserMapper : IMapper<UserDal, UserDom>, IMapper<UserDom, UserDal>
    {
        public UserDal Map(UserDom item) => throw new NotImplementedException();
        public UserDom Map(UserDal item) => throw new NotImplementedException();
    }
}
