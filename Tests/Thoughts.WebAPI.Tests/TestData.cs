using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using Thoughts.Domain.Base.Entities;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.Services;

namespace Thoughts.WebAPI.Tests
{
    public static class TestData
    {
        public static IEnumerable<Post> _Posts;
        public static Mock<IRepository<Post>> _Post_Repo_Mock;

        public static Tag[] _Tags;
        public static Mock<INamedRepository<Tag>> _Tag_Repo_Mock;

        public static Category[] _Categories;
        public static Mock<INamedRepository<Category>> _Category_Repo_Mock;

        public static User[] _Users;
        public static Mock<IRepository<User, string>> _User_Repo_Mock;

        public static RepositoryBlogPostManager _BlogPostManager;

        static TestData()
        {
            _Tags = new Tag[]
            {
                new (){ Id = 1, Name = "Tag1", },
                new (){ Id = 2, Name = "Tag2", },
                new() { Id = 3, Name = "Tag3", },
            };

            _Categories = new Category[]
            {
                new(){ Id = 1, Name = "Category1", },
                new(){ Id = 2, Name = "Category2", },
                new(){ Id = 3, Name = "Category3", },
            };

            _Users = new User[]
            {
            new()
            {
                Id = "1",
                FirstName = "User1",
                LastName = "User1",
                NickName="User1"
            },
            new()
            {
                Id = "2",
                FirstName = "User2",
                LastName = "User2",
                NickName = "User2"
            },
            new()
            {
                Id = "3",
                FirstName = "User3",
                LastName = "User3",
                NickName = "User3"
            },
            };

            _Posts = new[]
            {
                new Post
                {
                    Id = 1,
                    Title = "Title1",
                    Body = "Body1",
                    Tags = _Tags.Where(t => t.Id == 1).ToArray(),
                    Category = _Categories.First(c => c.Id == 1),
                    User = _Users.First(u => u.Id == "1"),
                },
                new Post
                {
                    Id = 2,
                    Title = "Title2",
                    Body = "Body2",
                    Tags = _Tags.Where(t => t.Id == 2).ToArray(),
                    Category = _Categories.First(c => c.Id == 1),
                    User = _Users.First(u => u.Id == "1"),
                },
                new Post
                {
                    Id = 3,
                    Title = "Title3",
                    Body = "Body3",
                    Tags = _Tags.Where(t => t.Id == 3).ToArray(),
                    Category = _Categories.First(c => c.Id == 1),
                    User = _Users.First(u => u.Id == "2"),
                },
                new Post
                {
                    Id = 4,
                    Title = "Title4",
                    Body = "Body4",
                    Tags = _Tags.Where(t => t.Id == 1).ToArray(),
                    Category = _Categories.First(c => c.Id == 2),
                    User = _Users.First(u => u.Id == "2"),
                },
                new Post
                {
                    Id = 5,
                    Title = "Title5",
                    Body = "Body5",
                    Tags = _Tags.Where(t => t.Id == 2).ToArray(),
                    Category = _Categories.First(c => c.Id == 3),
                    User = _Users.First(u => u.Id == "3"),
                },
                new Post
                {
                    Id = 6,
                    Title = "Title6",
                    Body = "Body6",
                    Tags = _Tags.Where(t => t.Id == 3).ToArray(),
                    Category = _Categories.First(c => c.Id == 1),
                    User = _Users.First(u => u.Id == "3"),
                },
                new Post
                {
                    Id = 7,
                    Title = "Title7",
                    Body = "Body7",
                    Tags = new[] { _Tags[0], _Tags[2] },
                    Category = _Categories[1],
                    User = _Users.First(u => u.Id == "1"),
                },
                new Post
                {
                    Id = 8,
                    Title = "Title8",
                    Body = "Body8",
                    Tags = null,
                    Category = _Categories[1],
                    User = _Users.First(u => u.Id == "2"),
                }
            };

            _Post_Repo_Mock = new Mock<IRepository<Post>>();
            _Tag_Repo_Mock = new Mock<INamedRepository<Tag>>();
            _Category_Repo_Mock = new Mock<INamedRepository<Category>>();
            _User_Repo_Mock = new Mock<IRepository<User, string>>();

            _BlogPostManager = new RepositoryBlogPostManager(
                _Post_Repo_Mock.Object,
                _Tag_Repo_Mock.Object,
                _Category_Repo_Mock.Object,
                _User_Repo_Mock.Object
            );
        }

    }
}
