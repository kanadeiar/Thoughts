using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace Thoughts.UI.MVC.Infrastructure.AutoMapper
{
    public class BlogDetailsWebModelProfile : Profile
    {
        public BlogDetailsWebModelProfile()
        {
            CreateMap<Post, BlogDetailsWebModel>()
                .ForMember(model => model.CategoryName, opt => opt.MapFrom(post => post.Category.Name))
                .ForMember(model => model.Title, opt => 
                    opt.MapFrom(post => post.Title))
                .ForMember(model => model.Body, opt => 
                    opt.MapFrom(post => post.Body))
                .ForMember(model => model.PostId, opt => 
                    opt.MapFrom(post => post.Id))
                .ForMember(model => model.UserId, opt => 
                    opt.MapFrom(post => post.User.Id))
                .ReverseMap();
        }
    }
}
