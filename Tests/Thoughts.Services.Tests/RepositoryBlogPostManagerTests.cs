using Moq;

using Thoughts.Domain;
using Thoughts.Domain.Base.Entities;
using Thoughts.Interfaces;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.Tests;

[TestClass]
public class RepositoryBlogPostManagerTests
{
    #region Test Initialize

    private Post[] _Posts;
    private Mock<IRepository<Post>> _Post_Repo_Mock;

    private Tag[] _Tags;
    private Mock<INamedRepository<Tag>> _Tag_Repo_Mock;

    private Category[] _Categories;
    private Mock<INamedRepository<Category>> _Category_Repo_Mock;

    private Status[] _Statuses;
    private Mock<INamedRepository<Status>> _Status_Repo_Mock;

    private User[] _Users;
    private Mock<IRepository<User, string>> _User_Repo_Mock;

    private IBlogPostManager _BlogPostManager;

    [TestInitialize]
    public void TestInitialize()
    {


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

        _Posts = new Post[]
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
                Tags = new[] {_Tags[0], _Tags[2] },
                Category = _Categories[1],
                User = _Users.First(u => u.Id == "1"),
            },
        };

        _Post_Repo_Mock = new Mock<IRepository<Post>>();
        _Tag_Repo_Mock = new Mock<INamedRepository<Tag>>();
        _Category_Repo_Mock = new Mock<INamedRepository<Category>>();
        _Status_Repo_Mock = new Mock<INamedRepository<Status>>();
        _User_Repo_Mock = new Mock<IRepository<User, string>>();


        _BlogPostManager = new RepositoryBlogPostManager(
            _Post_Repo_Mock.Object,
            _Tag_Repo_Mock.Object,
            _Category_Repo_Mock.Object,
            _Status_Repo_Mock.Object,
            _User_Repo_Mock.Object
            );
    }

    #endregion

    #region Get All Posts Tests

    [TestMethod]
    public async Task GetAllPostsAsync_Test_Returns_Posts_ToArray()
    {
        var expected_posts = _Posts;
        _Post_Repo_Mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_posts);

        var actual_posts = await _BlogPostManager.GetAllPostsAsync();

        CollectionAssert.AreEqual(expected_posts, actual_posts.ToArray());
    }

    [TestMethod]
    public async Task GetAllPostsCountAsync_Test_Returns_CountOfAllPosts()
    {
        var posts = _Posts;
        var expected_posts_count = posts.Length;

        _Post_Repo_Mock.Setup(c => c.GetCount(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_posts_count);

        var actual_posts_count = await _BlogPostManager.GetAllPostsCountAsync();

        Assert.AreEqual(expected_posts_count, actual_posts_count);
    }

    [TestMethod]
    public async Task GetAllPostsSkipTakeAsync_Test_Returns_EmptyEnumerable_when_Take_eq_0()
    {
        int skip = 2;
        int take = 0; //проверяем If: Take == 0

        var expected_page = _Posts.ToArray().Skip(skip).Take(take);

        _Post_Repo_Mock.Setup(c => c.Get(skip, take, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsSkipTakeAsync(skip, take);

        Assert.AreEqual(expected_page.Count(), actual_page.Count());
    }

    [TestMethod]
    public async Task GetAllPostsSkipTakeAsync_Test_Returns_EnumPageOfPosts()
    {
        int skip = 2;
        int take = 3;

        var expected_page = _Posts.Skip(skip).Take(take);

        _Post_Repo_Mock.Setup(c => c.Get(skip, take, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsSkipTakeAsync(skip, take);

        CollectionAssert.AreEqual(expected_page.ToArray(), actual_page.ToArray());
    }

    [TestMethod]
    public async Task GetAllPostsPageAsync_Test_Returns_EmptyPage()
    {
        var total_count = _Posts.Length;

        int pageIndex = 0;
        int pageSize = 3;

        var expected_page = new Page<Post>(Enumerable.Empty<Post>(), pageIndex, pageSize, total_count);

        _Post_Repo_Mock.Setup(c => c.GetPage(pageIndex, pageSize, It.IsAny<CancellationToken>()));

        var actual_page = await _BlogPostManager.GetAllPostsPageAsync(pageIndex, pageSize);

        Assert.AreEqual(expected_page.Items, actual_page.Items);
    }

    [TestMethod]
    public async Task GetAllPostsPageAsync_Test_Returns_Page()
    {
        var total_count = _Posts.Count();
        int pageIndex = 3;
        int pageSize = 3;
        var posts = _Posts;

        var expected_page = new Page<Post>(posts,
                                           pageIndex,
                                           pageSize,
                                           total_count);

        _Post_Repo_Mock.Setup(c => c.GetPage(pageIndex, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsPageAsync(pageIndex, pageSize);

        Assert.AreEqual(expected_page, actual_page);
    }

    #endregion

    #region Get All Posts by User Tests

    [TestMethod]
    public async Task GetAllPostsByUserIdAsync_Test_Returns_AllUserPosts()
    {
        var user_id = "1";

        var expected_posts = _Posts.Where(p => p.UserId == user_id);

        _Post_Repo_Mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(expected_posts);

        var actual_posts = await _BlogPostManager.GetAllPostsByUserIdAsync(user_id);

        CollectionAssert.AreEqual(expected_posts.ToArray(), actual_posts.ToArray());
    }

    [TestMethod]
    public async Task GetUserPostsCountAsync_Test_Returns_UserPostsCount()
    {
        var user_id = "2";
        var posts = _Posts.Where(p => p.UserId == user_id);
        var expected_posts_count = posts.Count();

        _Post_Repo_Mock.Setup(c => c.GetCount(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_posts_count);

        var actual_posts_count = await _BlogPostManager.GetUserPostsCountAsync(user_id);

        Assert.AreEqual(expected_posts_count, actual_posts_count);
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdSkipTakeAsync_Test_Returns_EmptyEnumerable()
    {
        var user_id = "3";
        var skip = 2;
        var take = 0; //проверяем If: Take == 0

        var expected_page = _Posts.Where(p => p.User.Id == user_id).Skip(skip).Take(take);

        _Post_Repo_Mock.Setup(c => c.Get(skip, take, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdSkipTakeAsync(user_id, skip, take);

        Assert.AreEqual(expected_page.Count(), actual_page.Count());
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdSkipTakeAsync_Test_Returns_CorrectEnum()
    {
        var user_id = "1";
        int skip = 1;
        int take = 3;

        var expected_page = _Posts.Where(p => p.User.Id == user_id).Skip(skip).Take(take);

        _Post_Repo_Mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_Posts);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdSkipTakeAsync( user_id, skip, take);

        CollectionAssert.AreEqual(expected_page.ToArray(), actual_page.ToArray());
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdPageAsync_Test_Returns_EmptyPage()
    {
        var user_id = "1";
        var total_count = _Posts.Where(c=>c.User.Id == user_id).Count();

        int pageIndex = 0;
        int pageSize = 3;

        var expected_page = new Page<Post>(Enumerable.Empty<Post>(), pageIndex, pageSize, total_count);

        _Post_Repo_Mock.Setup(c => c.GetPage(pageIndex, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdPageAsync(user_id, pageIndex, pageSize);

        Assert.AreEqual(expected_page.Items, actual_page.Items);
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdPageAsync_Test_Returns_CorrectPage()
    {
        var user_id = "1";
        var posts = _Posts.Where(p=>p.User.Id == user_id);
        var total_count = posts.Count();
        int pageIndex = 2;
        int pageSize = 3;

        var expected_page = new Page<Post>(posts, pageIndex, pageSize, total_count);


        //todo: не понимаю как настроить Mock, тест не проходит
        _Post_Repo_Mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(posts);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdPageAsync(user_id, pageIndex, pageSize);

        Assert.AreEqual(expected_page, actual_page);
    }

    #endregion

}
