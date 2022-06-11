using Moq;

using Thoughts.Domain.Base.Entities;
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.Tests;

[TestClass]
public class RepositoryBlogPostManagerTests
{
    private Post[] _Posts;
    private Mock<IRepository<Post>> _Post_Repo_Mock;

    private Tag[] _Tags;
    private Mock<INamedRepository<Tag>> _Tag_Repo_Mock;

    private Category[] _Categories;
    private Mock<INamedRepository<Category> > _Category_Repo_Mock;
    
    private Status[] _Statuses;
    private Mock<INamedRepository<Status>> _Status_Repo_Mock;

    private User[] _Users;
    private Mock<IRepository<User, string>> _User_Repo_Mock;

    private IBlogPostManager _BlogPostManager;

    [TestInitialize]
    public void TestInitialize()
    {
        _Posts = new Post[]
        {
            new()
            {
                Id = 1,
                Title = "Title1",
                Body = "Body1",
                Tags = (ICollection<Tag>)_Tags.First(t => t.Id == 1),
                Category = _Categories.First(c => c.Id == 1),
                User = _Users.First(u => u.Id == "1"),
            },
            new()
            {
                Id = 2,
                Title = "Title2",
                Body = "Body2",
                Tags = (ICollection<Tag>)_Tags.First(t => t.Id == 2),
                Category = _Categories.First(c => c.Id == 1),
                User = _Users.First(u => u.Id == "1"),
            },
            new()
            {
                Id = 3,
                Title = "Title3",
                Body = "Body3",
                Tags = (ICollection<Tag>)_Tags.First(t => t.Id == 3),
                Category = _Categories.First(c => c.Id == 1),
                User = _Users.First(u => u.Id == "2"),
            },
            new()
            {
                Id = 4,
                Title = "Title4",
                Body = "Body4",
                Tags = (ICollection<Tag>)_Tags.First(t => t.Id == 1),
                Category = _Categories.First(c => c.Id == 2),
                User = _Users.First(u => u.Id == "2"),
            },
            new()
            {
                Id = 5,
                Title = "Title5",
                Body = "Body5",
                Tags = (ICollection<Tag>)_Tags.First(t => t.Id == 2),
                Category = _Categories.First(c => c.Id == 3),
                User = _Users.First(u => u.Id == "3"),
            },
            new()
            {
                Id = 6,
                Title = "Title6",
                Body = "Body6",
                Tags = (ICollection<Tag>)_Tags.First(t => t.Id == 3),
                Category = _Categories.First(c => c.Id == 1),
                User = _Users.First(u => u.Id == "3"),
            },
        };

        _Tags = new Tag[]
        {
            new(){ Id = 1, Name = "Tag1", },
            new(){ Id = 2, Name = "Tag2", },
            new(){ Id = 3, Name = "Tag3", },
        };

        _Categories = new Category[]
        {
            new(){ Id = 1, Name = "Category1", },
            new(){ Id = 2, Name = "Category2", },
            new(){ Id = 3, Name = "Category3", },
        };

        _Statuses = new Status[]
        {
            new(){ Id = 1, Name = "Status1", },
            new(){ Id = 2, Name = "Status2", },
            new(){ Id = 3, Name = "Status3", },
        };

        _Users = new User[]
        {
            new(){ Id = "1",
                FirstName = "User1",
                LastName = "User1",
                NickName="User1"
            },
            new(){ Id = "2",
                FirstName = "User2",
                LastName = "User2",
                NickName="User2"
            },
            new(){ Id = "3",
                FirstName = "User3",
                LastName = "User3",
                NickName="User3"
            },
        };

        _Post_Repo_Mock = new Mock<IRepository<Post>>();
        _Post_Repo_Mock.Setup(p => p.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_Posts);

        _Tag_Repo_Mock = new Mock<INamedRepository<Tag>>();
        _Tag_Repo_Mock.Setup(t => t.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_Tags);

        _Category_Repo_Mock = new Mock<INamedRepository<Category>>();
        _Category_Repo_Mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_Categories);

        _Status_Repo_Mock = new Mock<INamedRepository<Status>>();
        _User_Repo_Mock = new Mock<IRepository<User, string>>();
            .ReturnsAsync(_Statuses);

        _User_Repo_Mock = new Mock<IRepository<User, string>>();
        _User_Repo_Mock.Setup(u => u.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_Users);

        _BlogPostManager = new RepositoryBlogPostManager(
            _Post_Repo_Mock.Object,
            _Tag_Repo_Mock.Object,
            _Category_Repo_Mock.Object,
            _Status_Repo_Mock.Object,
            _User_Repo_Mock.Object
            );
    }

    [TestMethod]
    public async Task GetAllPostsAsync_Test_Returns_Posts_ToArray()
    {
        Post[] expected_posts =
        {
            new() { Title = "Post1" },
            new() { Title = "Post2" },
            new() { Title = "Post3" },
            new() { Title = "Post4" },
        };

        var post_repo_mock = new Mock<IRepository<Post>>();
        post_repo_mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_posts);

        var tag_repo_mock = new Mock<INamedRepository<Tag>>();
        var category_repo_mock = new Mock<INamedRepository<Category>>();
        var statuses_repo_mock = new Mock<INamedRepository<Status>>();
        var users_repo_mock = new Mock<IRepository<User, string>>();

        var manager = new RepositoryBlogPostManager(
            post_repo_mock.Object,
            tag_repo_mock.Object,
            category_repo_mock.Object,
            statuses_repo_mock.Object,
            users_repo_mock.Object);

        var actual_posts = await manager.GetAllPostsAsync();

        CollectionAssert.AreEqual(expected_posts, actual_posts.ToArray());
    }
}
