using Thoughts.Extensions.Base;

using PostDal = Thoughts.DAL.Entities.Post;
using StatusDal = Thoughts.DAL.Entities.Status;
using CategoryDal = Thoughts.DAL.Entities.Category;
using PostDom = Thoughts.Domain.Base.Entities.Post;
using StatusDom = Thoughts.Domain.Base.Entities.Status;
using CategoryDom = Thoughts.Domain.Base.Entities.Category;

namespace Thoughts.Extensions.Maps
{
    public class PostMapper : IMapper<PostDal, PostDom>, IMapper<PostDom, PostDal>
    {
        public PostDom Map(PostDal item)
        {
            int st = (int)item.Status;
            return new PostDom()
            {
                Id = item.Id,
                Status = (StatusDom)st,
                Date = item.Date,
                User = new Domain.Base.Entities.User(),//
                UserId = item.User.Id,//
                Title = item.Title,
                Body = item.Body,
                Category = new CategoryDom(),//
                Tags = item.Tags()
            };
        }
        public PostDal Map(PostDom item) => throw new NotImplementedException();
    }
    public class CategoryMapper : IMapper<CategoryDal, CategoryDom>, IMapper<CategoryDom, CategoryDal>
    {
        public CategoryDal Map(CategoryDom item) => throw new NotImplementedException();
        public CategoryDom Map(CategoryDal item) => throw new NotImplementedException();
    }
}
