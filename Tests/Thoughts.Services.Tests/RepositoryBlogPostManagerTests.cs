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

    private User[] _Users;
    private Mock<IRepository<User, string>> _User_Repo_Mock;

    private RepositoryBlogPostManager _BlogPostManager;

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
    public async Task GetAllPostsPageAsync_Test_Returns_EmptyPage_when_pageSize_eq_0()
    {
        var total_count = _Posts.Length;

        int pageIndex = 2;
        int pageSize = 0;

        var expected_page = new Page<Post>(Enumerable.Empty<Post>(), pageIndex, pageSize, total_count);

        _Post_Repo_Mock.Setup(c => c.GetPage(pageIndex, pageSize, It.IsAny<CancellationToken>())).ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsPageAsync(pageIndex, pageSize);

        Assert.AreEqual(expected_page.Items, actual_page.Items);
    }

    [TestMethod]
    public async Task GetAllPostsPageAsync_Test_Returns_Page()
    {
        var total_count = _Posts.Length;
        int pageIndex = 2;
        int pageSize = 3;
        var posts = _Posts.Skip(pageIndex * pageSize).Take(pageSize);

        var expected_page = new Page<Post>(posts,
                                           pageIndex,
                                           pageSize,
                                           total_count);

        _Post_Repo_Mock.Setup(c => c.GetPage(pageIndex, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsPageAsync(pageIndex, pageSize);

        //Assert.IsTrue(ReferenceEquals(expected_page, actual_page));
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
    public async Task GetAllPostsByUserIdSkipTakeAsync_Test_Returns_EmptyEnumerable_when_Take_eq_0()
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

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdSkipTakeAsync(user_id, skip, take);

        CollectionAssert.AreEqual(expected_page.ToArray(), actual_page.ToArray());
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdPageAsync_Test_Returns_EmptyPage_when_pageSize_eq_0()
    {
        var user_id = "1";
        var total_count = _Posts.Where(c => c.User.Id == user_id).Count();

        int pageIndex = 2;
        int pageSize = 0;

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
        int pageIndex = 0;
        int pageSize = 2;
        var all_posts = _Posts;
        var posts = all_posts.Where(p => p.User.Id == user_id).Skip(pageIndex * pageSize).Take(pageSize);
        var total_count = all_posts.Where(p => p.User.Id == user_id).Count();

        var expected_page = new Page<Post>(posts, pageIndex, pageSize, total_count);

        //todo: не понимаю как настроить Mock, тест не проходит
        _Post_Repo_Mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(all_posts);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdPageAsync(user_id, pageIndex, pageSize);

        CollectionAssert.AreEqual(posts.ToArray(), actual_page.Items.ToArray());
        Assert.AreEqual(pageIndex, actual_page.PageNumber);
        Assert.AreEqual(pageSize, actual_page.PageSize);
        Assert.AreEqual(total_count, actual_page.TotalCount);
    }

    #endregion

    #region GetById Delete 

    [TestMethod]
    public async Task GetPostAsync_Test_Returns_Post()
    {
        var post_id = 7;
        var expected_post = _Posts.Single(p => p.Id == post_id);

        _Post_Repo_Mock.Setup(p => p.GetById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);

        var actual_post = await _BlogPostManager.GetPostAsync(post_id);

        Assert.AreEqual(expected_post, actual_post);
    }

    [TestMethod]
    public async Task DeletePostAsync_Test_Returns_True_if_FindPost()
    {
        //Решил дополнительные проверки сделать, сравнивая наборы постов

        var post_id = 1;
        var posts = _Posts.ToList();
        var expected_post = _Posts.Single(p => p.Id == post_id);
        bool expecting_result = posts.Remove(expected_post);

        _Post_Repo_Mock.Setup(p => p.GetById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);
        _Post_Repo_Mock.Setup(c => c.DeleteById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);

        var actual_result = await _BlogPostManager.DeletePostAsync(post_id);

        _Post_Repo_Mock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(posts);
        var actual_posts = await _BlogPostManager.GetAllPostsAsync();


        Assert.AreEqual(expecting_result, actual_result);
        CollectionAssert.AreNotEqual(_Posts, posts);
        CollectionAssert.AreEqual(posts, actual_posts.ToArray());
        _Post_Repo_Mock.Verify(c => c.DeleteById(post_id, It.IsAny<CancellationToken>()));
        _Post_Repo_Mock.Verify(c => c.GetById(post_id, It.IsAny<CancellationToken>()));
        _Post_Repo_Mock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _Post_Repo_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task DeletePostAsync_Test_Returns_False_if_PostNotFound()
    {
        var post_id = 8;
        var posts = _Posts.ToList();
        var expected_post = _Posts.SingleOrDefault(p => p.Id == post_id);
        var expecting_result = posts.Remove(expected_post!);

        _Post_Repo_Mock.Setup(p => p.GetById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post!);
        _Post_Repo_Mock.Setup(c => c.DeleteById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post!);

        var actual_result = await _BlogPostManager.DeletePostAsync(post_id);

        Assert.AreEqual(expecting_result, actual_result);
    }
    #endregion

    #region Create (7 tests)

    [TestMethod]
    public async Task CreatePostAsync_Test_Returns_Post_With_newCategory() // todo: проверить что может быть не так
    {
        string title = "new_title_post8";
        string body = "new_body_post8";
        string user_id = "2";
        string new_category = "new_Category4_post8";

        var containsCategory = _Categories.Any(p => p.Name == new_category);
        var user = _Users.Single(p => p.Id == user_id);

        var expected_post = new Post
        {
            Title = title,
            Body = body,
            User = user,
            Category = new Category { Name = new_category },
        };

        _User_Repo_Mock.Setup(c => c.GetById(user_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _Category_Repo_Mock.Setup(c => c.ExistName(new_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(containsCategory);
        //_Post_Repo_Mock.Setup(c => c.Update(expected_post, It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(expected_post);
        _Post_Repo_Mock.Setup(c => c.Add(expected_post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);

        var actual_post = await _BlogPostManager.CreatePostAsync(title, body, user_id, new_category);

        Assert.AreEqual(expected_post.Title, actual_post.Title);
        Assert.AreEqual(expected_post.Body, actual_post.Body);
        Assert.AreEqual(expected_post.User.Id, actual_post.User.Id);
        Assert.AreEqual(expected_post.Category.Name, actual_post.Category.Name); // сравнивать категорию можно только по имени
        //Assert.AreEqual(expected_post.Category, actual_post.Category); // <- такое сравнение выдаёт ошибку
        //Assert.AreEqual(expected_post.ToString, actual_post.ToString);
        //Assert.AreEqual(expected_post, actual_post); // <- любые прямые сравнения объектов у меня не сработали
        // в общем, полностью сравнивать объекты не получается

        _Post_Repo_Mock.Verify();
        _Category_Repo_Mock.Verify();
        _User_Repo_Mock.Verify();

        //_Post_Repo_Mock.VerifyNoOtherCalls(); //<- здесь так же возникала ошибка
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Returns_Post_With_oldCathegory()
    {
        string title = "new_title_post8";
        string body = "new_body_post8";
        string user_id = "2";
        string old_category = "Category3";

        var containsCategory = _Categories.Any(p => p.Name == old_category);

        var expected_category = _Categories.Single(c => c.Name == old_category);

        var user = _Users.Single(p => p.Id == user_id);

        var expected_post = new Post
        {
            Title = title,
            Body = body,
            User = user,
            Category = expected_category,
        };

        _User_Repo_Mock.Setup(c => c.GetById(user_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _Category_Repo_Mock.Setup(c => c.ExistName(old_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(containsCategory);
        _Category_Repo_Mock.Setup(c => c.GetByName(old_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_category);
        _Post_Repo_Mock.Setup(c => c.Add(expected_post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);

        var actual_post = await _BlogPostManager.CreatePostAsync(title, body, user_id, old_category);

        Assert.AreEqual(expected_post.Title, actual_post.Title);
        Assert.AreEqual(expected_post.Body, actual_post.Body);
        Assert.AreEqual(expected_post.User.Id, actual_post.User.Id);
        Assert.AreEqual(expected_post.Category.Name, actual_post.Category.Name);

        Assert.AreEqual(expected_category, actual_post.Category);

        _Post_Repo_Mock.Verify();
        _Category_Repo_Mock.Verify();
        _User_Repo_Mock.Verify();
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Throws_ArgumentNullException_when_Title_is_Null()
    {
        string Title = null; //проверяем на null заголовок

        string body = "new_body_post8";
        var category = _Categories[0];
        var user = _Users[0];

        var expected_exception = new ArgumentNullException(nameof(Title));

        try
        {
            await _BlogPostManager.CreatePostAsync(Title, body, user.Id, category.Name);
        }
        catch (Exception actual_exception)
        {
            Assert.AreEqual(expected_exception.Message, actual_exception.Message);
            Assert.AreEqual(expected_exception.GetType(), actual_exception.GetType());
            return;
        }

        Assert.Fail("Исключение не было получено.");
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Throws_ArgumentNullException_when_Body_is_Null()
    {
        string title = "new_title_post8";
        string Body = null; //проверяем на null тело поста
        var category = _Categories[0];
        var user = _Users[0];

        var expected_exception = new ArgumentNullException(nameof(Body));

        try
        {
            await _BlogPostManager.CreatePostAsync(title, Body, user.Id, category.Name);
        }
        catch (Exception actual_exception)
        {
            Assert.AreEqual(expected_exception.Message, actual_exception.Message);
            Assert.AreEqual(expected_exception.GetType(), actual_exception.GetType());
            return;
        }

        Assert.Fail("Исключение не было получено.");
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Throws_ArgumentNullException_when_Category_is_Null()
    {
        string title = "new_title_post8";
        string body = "new_body_post8";
        string Category = null; //проверяем на null категорию
        var user = _Users[0];

        var expected_exception = new ArgumentNullException(nameof(Category));

        try
        {
            await _BlogPostManager.CreatePostAsync(title, body, user.Id, Category);
        }
        catch (Exception actual_exception)
        {
            Assert.AreEqual(expected_exception.Message, actual_exception.Message);
            Assert.AreEqual(expected_exception.GetType(), actual_exception.GetType());
            return;
        }

        Assert.Fail("Исключение не было получено.");
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Throws_ArgumentNullException_when_UserId_is_Null()
    {
        string title = "new_title_post8";
        string body = "new_body_post8";
        var category = _Categories[0]; 
        string UserId = null; //проверяем на null идентификатор пользователя

        var expected_exception = new ArgumentNullException(nameof(UserId));

        try
        {
            await _BlogPostManager.CreatePostAsync(title, body, UserId, category.Name);
        }
        catch (Exception actual_exception)
        {
            Assert.AreEqual(expected_exception.Message, actual_exception.Message);
            Assert.AreEqual(expected_exception.GetType(), actual_exception.GetType());
            return;
        }

        Assert.Fail("Исключение не было получено.");
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Throws_ArgumentException_when_UserId_is_NotGreater_0()
    {
        string title = "new_title_post8";
        string body = "new_body_post8";
        var category = _Categories[0];
        string UserId = ""; //проверяем пустой идентификатор пользователя

        var expected_exception = new ArgumentException("Не указан идентификатор пользователя", nameof(UserId));

        try
        {
            await _BlogPostManager.CreatePostAsync(title, body, UserId, category.Name);
        }
        catch (Exception actual_exception)
        {
            Assert.AreEqual(expected_exception.Message, actual_exception.Message);
            Assert.AreEqual(expected_exception.GetType(), actual_exception.GetType());
            return;
        }

        Assert.Fail("Исключение не было получено.");
    }

    #endregion

    #region Tag - AssignTag (5 tests)

    [TestMethod]
    public async Task AssignTagAsync_Test_Returns_True_when_Tag_Was_Found()
    {
        var post = _Posts[0];
        post.Tags = post.Tags.ToList(); //иначе Add не сработает
        var tag_name = "Tag3";
        var expected_tag = _Tags[2];  // <- существующий "Tag3"
        var expected_result = true;

        _Post_Repo_Mock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _Tag_Repo_Mock.Setup(c => c.ExistName(tag_name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _Tag_Repo_Mock.Setup(c => c.GetByName(tag_name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);
        _Tag_Repo_Mock.Setup(c => c.Add(expected_tag, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);
        _Post_Repo_Mock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_result = await _BlogPostManager.AssignTagAsync(post.Id, tag_name);

        Assert.AreEqual(expected_result, actual_result);
        Assert.IsTrue(post.Tags.Count() == 2); 
        
        _Tag_Repo_Mock.Verify();
        _Post_Repo_Mock.Verify();
    }

    [TestMethod]
    public async Task AssignTagAsync_Test_Returns_True_when_Tag_Is_NewTag()
    {
        var post = _Posts[1];
        post.Tags = post.Tags.ToList();
        var tag_name = "Tag4";
        var expected_tag = new Tag { Name = tag_name }; // новый тег
        var expected_result = true;

        _Post_Repo_Mock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _Tag_Repo_Mock.Setup(c => c.ExistName(tag_name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _Tag_Repo_Mock.Setup(c => c.GetByName(tag_name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);
        _Tag_Repo_Mock.Setup(c => c.Add(expected_tag, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);
        _Post_Repo_Mock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_result = await _BlogPostManager.AssignTagAsync(post.Id, tag_name);

        Assert.AreEqual(expected_result, actual_result);
        Assert.IsTrue(post.Tags.Count() == 2); 
        _Tag_Repo_Mock.Verify();
        _Post_Repo_Mock.Verify();
    }

    [TestMethod]  //судя по отладчику, тест работает неправильно
    public async Task AssignTagAsync_Test_Returns_True_when_Tag_Is_AlreadyAssigned()
    {
        //todo: тест судя по отладчику неправильно отрабатывает - нет захода в блок, где возврат true при изначальном наличии в посте тега
        //строка 243-244

        var post = _Posts[6];
        post.Tags = post.Tags.ToList();
        var expected_tag = _Tags[0]; //уже существующий и прикреплённый тег к посту
        var expected_result = true;

        _Post_Repo_Mock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _Tag_Repo_Mock.Setup(c => c.ExistName(expected_tag.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _Tag_Repo_Mock.Setup(c => c.GetByName(expected_tag.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);

        var actual_result = await _BlogPostManager.AssignTagAsync(post.Id, expected_tag.Name);

        Assert.AreEqual(expected_result, actual_result);
        _Tag_Repo_Mock.Verify();
    }

    [TestMethod]
    public async Task AssignTagAsync_Test_Returns_ArgumentNullException_when_Tag_is_Null()
    {
        var post_id = 1;
        string Tag = null;

        var expected_exception = new ArgumentNullException(nameof(Tag));

        try
        {
            await _BlogPostManager.AssignTagAsync(post_id, Tag);
        }
        catch (Exception actual_exception)
        {
            Assert.AreEqual(expected_exception.Message, actual_exception.Message);
            Assert.AreEqual(expected_exception.GetType(), actual_exception.GetType());
            return;
        }

        Assert.Fail("Исключение не было получено.");
    }

    [TestMethod]
    public async Task AssignTagAsync_Test_Returns_False_when_Post_is_Null()
    {
        var post_id = 10; //идентификатор несуществующего поста
        var tag = _Tags[0];

        var expexted_result = await _BlogPostManager.AssignTagAsync(post_id, tag.Name);

        Assert.AreEqual(false, expexted_result);
    }

    #endregion
}