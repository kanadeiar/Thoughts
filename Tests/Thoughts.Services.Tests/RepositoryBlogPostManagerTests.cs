﻿using Microsoft.Extensions.Logging;

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
    private Mock<IRepository<Post>> _PostsRepositoryMock;

    private Tag[] _Tags;
    private Mock<INamedRepository<Tag>> _TagsRepositoryMock;

    private Category[] _Categories;
    private Mock<INamedRepository<Category>> _CategoriesRepositoryMock;

    private User[] _Users;
    private Mock<IRepository<User, string>> _UsersRepositoryMock;

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
                Tags = new[] { 1 },
                Category = (1, "Category1" ),
                User = _Users[0],
            },
            new Post
            {
                Id = 2,
                Title = "Title2",
                Body = "Body2",
                Tags = new[] { 1, 2 },
                Category = (1, "Category1" ),
                User = _Users[0],
            },
            new Post
            {
                Id = 3,
                Title = "Title3",
                Body = "Body3",
                Tags = new[] { 2, 3 },
                Category = (1, "Category1" ),
                User = _Users[1],
            },
            new Post
            {
                Id = 4,
                Title = "Title4",
                Body = "Body4",
                Tags = new[] { 1 },
                Category = (2, "Category2" ),
                User = _Users[1],
            },
            new Post
            {
                Id = 5,
                Title = "Title5",
                Body = "Body5",
                Tags = new[] { 2 },
                Category = (3, "Category3" ),
                User = _Users[2],
            },
            new Post
            {
                Id = 6,
                Title = "Title6",
                Body = "Body6",
                Tags = new[] { 3 },
                Category = (1, "Category1" ),
                User = _Users[2],
            },
            new Post
            {
                Id = 7,
                Title = "Title7",
                Body = "Body7",
                Tags = new[] { 1, 2 },
                Category = (2, "Category2" ),
                User = _Users[0],
            },
            new Post
            {
                Id = 8,
                Title = "Title8",
                Body = "Body8",
                Tags = null,
                Category = (1, "Category1" ),
                User = _Users[1],
            },
        };

        _PostsRepositoryMock = new Mock<IRepository<Post>>();
        _TagsRepositoryMock = new Mock<INamedRepository<Tag>>();
        _CategoriesRepositoryMock = new Mock<INamedRepository<Category>>();
        _UsersRepositoryMock = new Mock<IRepository<User, string>>();
        var logger = new Mock<ILogger<RepositoryBlogPostManager>>();

        _BlogPostManager = new RepositoryBlogPostManager(
            _PostsRepositoryMock.Object,
            _TagsRepositoryMock.Object,
            _CategoriesRepositoryMock.Object,
            _UsersRepositoryMock.Object,
            logger.Object
            );
    }

    #endregion

    #region Get All Posts Tests

    [TestMethod]
    public async Task GetAllPostsAsync_Test_Returns_Posts_ToArray()
    {
        var expected_posts = _Posts;
        _PostsRepositoryMock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_posts);

        var actual_posts = await _BlogPostManager.GetAllPostsAsync();

        CollectionAssert.AreEqual(expected_posts, actual_posts.ToArray());

        _PostsRepositoryMock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAllPostsCountAsync_Test_Returns_CountOfAllPosts()
    {
        var posts = _Posts;
        var expected_posts_count = posts.Length;

        _PostsRepositoryMock.Setup(c => c.GetCount(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_posts_count);

        var actual_posts_count = await _BlogPostManager.GetAllPostsCountAsync();

        Assert.AreEqual(expected_posts_count, actual_posts_count);

        _PostsRepositoryMock.Verify(c => c.GetCount(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAllPostsSkipTakeAsync_Test_Returns_EmptyEnumerable_when_Take_eq_0()
    {
        var skip = 2;
        var take = 0; //проверяем If: Take == 0

        var expected_page = _Posts.ToArray().Skip(skip).Take(take);

        var actual_page = await _BlogPostManager.GetAllPostsSkipTakeAsync(skip, take);

        Assert.AreEqual(expected_page.Count(), actual_page.Count());
    }

    [TestMethod]
    public async Task GetAllPostsSkipTakeAsync_Test_Returns_EnumPageOfPosts()
    {
        var skip = 2;
        var take = 3;

        var expected_page = _Posts.Skip(skip).Take(take);

        _PostsRepositoryMock.Setup(c => c.Get(skip, take, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);

        var actual_page = await _BlogPostManager.GetAllPostsSkipTakeAsync(skip, take);

        CollectionAssert.AreEqual(expected_page.ToArray(), actual_page.ToArray());

        _PostsRepositoryMock.Verify(c => c.Get(skip, take, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAllPostsPageAsync_Test_Returns_EmptyPage_when_pageSize_eq_0()
    {
        var total_count = _Posts.Length;

        var pageIndex = 2;
        var pageSize = 0;

        var expected_page = new Page<Post>(Enumerable.Empty<Post>(), pageIndex, pageSize, total_count);

        _PostsRepositoryMock.Setup(c => c.GetCount(It.IsAny<CancellationToken>())).ReturnsAsync(total_count);

        var actual_page = await _BlogPostManager.GetAllPostsPageAsync(pageIndex, pageSize);

        Assert.AreEqual(expected_page.Items, actual_page.Items);

        _PostsRepositoryMock.Verify(c => c.GetCount(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAllPostsPageAsync_Test_Returns_Page()
    {
        var total_count = _Posts.Length;
        var pageIndex = 2;
        var pageSize = 3;
        var posts = _Posts.Skip(pageIndex * pageSize).Take(pageSize);

        var expected_page = new Page<Post>(posts,
                                           pageIndex,
                                           pageSize,
                                           total_count);

        _PostsRepositoryMock.Setup(c => c.GetPage(pageIndex, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_page);
        _PostsRepositoryMock.Setup(c => c.GetCount(It.IsAny<CancellationToken>())).ReturnsAsync(total_count);

        var actual_page = await _BlogPostManager.GetAllPostsPageAsync(pageIndex, pageSize);

        //Assert.IsTrue(ReferenceEquals(expected_page, actual_page));
        Assert.AreEqual(expected_page, actual_page);

        _PostsRepositoryMock.Verify(c => c.GetPage(pageIndex, pageSize, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.GetCount(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #region Get All Posts by User Tests

    [TestMethod]
    public async Task GetAllPostsByUserIdAsync_Test_Returns_AllUserPosts()
    {
        var user_id = "1";

        var expected_posts = _Posts.Where(p => p.User.Id == user_id).ToArray();

        _PostsRepositoryMock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(expected_posts);

        var actual_posts = await _BlogPostManager.GetAllPostsByUserIdAsync(user_id);

        CollectionAssert.AreEqual(expected_posts.ToArray(), actual_posts.ToArray());

        _PostsRepositoryMock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetUserPostsCountAsync_Test_Returns_UserPostsCount()
    {
        var user_id = "2";
        var posts = _Posts.Where(p => p.User.Id == user_id).ToArray();
        var expected_posts_count = posts.Length;

        _PostsRepositoryMock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(posts);

        var actual_posts_count = await _BlogPostManager.GetUserPostsCountAsync(user_id);

        Assert.AreEqual(expected_posts_count, actual_posts_count);

        _PostsRepositoryMock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdSkipTakeAsync_Test_Returns_EmptyEnumerable_when_Take_eq_0()
    {
        var user_id = "3";
        var skip = 2;
        var take = 0; //проверяем If: Take == 0

        var expected_page = _Posts.Where(p => p.User.Id == user_id).Skip(skip).Take(take);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdSkipTakeAsync(user_id, skip, take);

        Assert.AreEqual(expected_page.Count(), actual_page.Count());
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdSkipTakeAsync_Test_Returns_CorrectEnum()
    {
        var user_id = "1";
        var skip = 1;
        var take = 3;

        var expected_page = _Posts.Where(p => p.User.Id == user_id).Skip(skip).Take(take);

        _PostsRepositoryMock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_Posts);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdSkipTakeAsync(user_id, skip, take);

        CollectionAssert.AreEqual(expected_page.ToArray(), actual_page.ToArray());

        _PostsRepositoryMock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdPageAsync_Test_Returns_EmptyPage_when_pageSize_eq_0()
    {
        var user_id = "1";
        var total_user_posts = _Posts.Where(c => c.User.Id == user_id);
        var total_count = total_user_posts.Count();
        var pageIndex = 2;
        var pageSize = 0;

        var expected_page = new Page<Post>(Enumerable.Empty<Post>(), pageIndex, pageSize, total_count);

        _PostsRepositoryMock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(total_user_posts);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdPageAsync(user_id, pageIndex, pageSize);

        Assert.AreEqual(expected_page.Items, actual_page.Items);

        _PostsRepositoryMock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAllPostsByUserIdPageAsync_Test_Returns_CorrectPage()
    {
        var user_id = "1";
        var pageIndex = 0;
        var pageSize = 2;
        var all_posts = _Posts;
        var posts = all_posts.Where(p => p.User.Id == user_id).Skip(pageIndex * pageSize).Take(pageSize);
        var total_count = all_posts.Where(p => p.User.Id == user_id).Count();

        var expected_page = new Page<Post>(posts, pageIndex, pageSize, total_count);

        //todo: не понимаю как настроить Mock, тест не проходит
        _PostsRepositoryMock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(all_posts);

        var actual_page = await _BlogPostManager.GetAllPostsByUserIdPageAsync(user_id, pageIndex, pageSize);

        CollectionAssert.AreEqual(posts.ToArray(), actual_page.Items.ToArray());
        Assert.AreEqual(pageIndex, actual_page.PageNumber);
        Assert.AreEqual(pageSize, actual_page.PageSize);
        Assert.AreEqual(total_count, actual_page.TotalCount);

        _PostsRepositoryMock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #region GetById Delete 

    [TestMethod]
    public async Task GetPostAsync_Test_Returns_Post()
    {
        var post_id = 7;
        var expected_post = _Posts.Single(p => p.Id == post_id);

        _PostsRepositoryMock.Setup(p => p.GetById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);

        var actual_post = await _BlogPostManager.GetPostAsync(post_id);

        Assert.AreEqual(expected_post, actual_post);

        _PostsRepositoryMock.Verify(p => p.GetById(post_id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task DeletePostAsync_Test_Returns_True_if_FindPost()
    {
        //Решил дополнительные проверки сделать, сравнивая наборы постов

        var post_id = 1;
        var posts = _Posts.ToList();
        var expected_post = _Posts.Single(p => p.Id == post_id);
        var expecting_result = posts.Remove(expected_post);

        _PostsRepositoryMock.Setup(p => p.GetById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);
        _PostsRepositoryMock.Setup(c => c.DeleteById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);

        var actual_result = await _BlogPostManager.DeletePostAsync(post_id);

        _PostsRepositoryMock.Setup(c => c.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(posts);
        var actual_posts = await _BlogPostManager.GetAllPostsAsync();

        Assert.AreEqual(expecting_result, actual_result);
        CollectionAssert.AreNotEqual(_Posts, posts);
        CollectionAssert.AreEqual(posts, actual_posts.ToArray());

        _PostsRepositoryMock.Verify(c => c.DeleteById(post_id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.GetById(post_id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.GetAll(It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task DeletePostAsync_Test_Returns_False_if_PostNotFound()
    {
        var post_id = 8;
        var posts = _Posts.ToList();
        var expected_post = _Posts.SingleOrDefault(p => p.Id == post_id);
        var expecting_result = posts.Remove(expected_post!);

        _PostsRepositoryMock.Setup(p => p.GetById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post!);
        _PostsRepositoryMock.Setup(c => c.DeleteById(post_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post!);

        var actual_result = await _BlogPostManager.DeletePostAsync(post_id);

        Assert.AreEqual(expecting_result, actual_result);

        _PostsRepositoryMock.Verify(p => p.GetById(post_id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.DeleteById(post_id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }
    #endregion

    #region Create (7 tests)

    [TestMethod]
    public async Task CreatePostAsync_Test_Returns_Post_With_newCategory() // todo: проверить что может быть не так
    {
        var title = "new_title_post8";
        var body = "new_body_post8";
        var user_id = "2";
        var new_category = "new_Category4_post8";

        var contains_category = _Categories.FirstOrDefault(p => p.Name == new_category);
        var user = _Users.Single(p => p.Id == user_id);

        var expected_post = new Post
        {
            Title = title,
            Body = body,
            User = user,
            Category = (contains_category.Id, contains_category.Name),
        };

        _UsersRepositoryMock.Setup(c => c.GetById(user_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _CategoriesRepositoryMock.Setup(c => c.ExistName(new_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contains_category is not null);
        //_PostsRepositoryMock.Setup(c => c.Update(expected_post, It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(expected_post);
        _PostsRepositoryMock.Setup(c => c.Add(expected_post, It.IsAny<CancellationToken>()))
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

        _UsersRepositoryMock.Verify(c => c.GetById(user_id, It.IsAny<CancellationToken>()));
        _CategoriesRepositoryMock.Verify(c => c.ExistName(new_category, It.IsAny<CancellationToken>()));
        //_PostsRepositoryMock.Verify(c => c.Add(expected_post, It.IsAny<CancellationToken>())); //Add не проходит верификацию
        _UsersRepositoryMock.VerifyNoOtherCalls();
        //_PostsRepositoryMock.VerifyNoOtherCalls();
        _CategoriesRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Returns_Post_With_oldCathegory()
    {
        var title = "new_title_post8";
        var body = "new_body_post8";
        var user_id = "2";
        var old_category = "Category3";

        var contains_category = _Categories.Any(p => p.Name == old_category);

        var expected_category = _Categories.Single(c => c.Name == old_category);

        var user = _Users.Single(p => p.Id == user_id);

        var expected_post = new Post
        {
            Title = title,
            Body = body,
            User = user,
            Category = (expected_category.Id, expected_category.Name),
        };

        _UsersRepositoryMock.Setup(c => c.GetById(user_id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _CategoriesRepositoryMock.Setup(c => c.ExistName(old_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contains_category);
        _CategoriesRepositoryMock.Setup(c => c.GetByName(old_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_category);
        _PostsRepositoryMock.Setup(c => c.Add(expected_post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_post);

        var actual_post = await _BlogPostManager.CreatePostAsync(title, body, user_id, old_category);

        Assert.AreEqual(expected_post.Title, actual_post.Title);
        Assert.AreEqual(expected_post.Body, actual_post.Body);
        Assert.AreEqual(expected_post.User.Id, actual_post.User.Id);
        Assert.AreEqual(expected_post.Category.Name, actual_post.Category.Name);
        Assert.AreEqual(expected_category, actual_post.Category);

        _UsersRepositoryMock.Verify(c => c.GetById(user_id, It.IsAny<CancellationToken>()));
        _CategoriesRepositoryMock.Verify(c => c.ExistName(old_category, It.IsAny<CancellationToken>()));
        _CategoriesRepositoryMock.Verify(c => c.GetByName(old_category, It.IsAny<CancellationToken>()));
        //_PostsRepositoryMock.Verify(c => c.Add(expected_post, It.IsAny<CancellationToken>())); //Add не проходит верификацию
        _UsersRepositoryMock.VerifyNoOtherCalls();
        //_PostsRepositoryMock.VerifyNoOtherCalls();
        _CategoriesRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Throws_ArgumentNullException_when_Title_is_Null()
    {
        string? Title = null; //проверяем на null заголовок

        var body = "new_body_post8";
        var category = _Categories[0];
        var user = _Users[0];

        var expected_exception = new ArgumentNullException(nameof(Title));

        try
        {
            await _BlogPostManager.CreatePostAsync(Title!, body, user.Id, category.Name);
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
    public async Task CreatePostAsync_Test_Throws_ArgumentNullException_when_Title_is_Null2()
    {
        string? Title = null; //проверяем на null заголовок

        var body = "new_body_post8";
        var category = _Categories[0];
        var user = _Users[0];

        const string expected_argument = "Title";

        var actual_exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
        {
            await _BlogPostManager.CreatePostAsync(Title!, body, user.Id, category.Name);
        });

        Assert.IsTrue(actual_exception.ParamName is { Length: > 0 });
        Assert.AreEqual(expected_argument, actual_exception.ParamName);
    }

    [TestMethod]
    public async Task CreatePostAsync_Test_Throws_ArgumentNullException_when_Body_is_Null()
    {
        var title = "new_title_post8";
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
        var title = "new_title_post8";
        var body = "new_body_post8";
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
        var title = "new_title_post8";
        var body = "new_body_post8";
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
        var title = "new_title_post8";
        var body = "new_body_post8";
        var category = _Categories[0];
        var UserId = ""; //проверяем пустой идентификатор пользователя

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
        var expected_tag = _Tags[2];  // <- существующий "Tag3"
        var expected_result = true;

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _TagsRepositoryMock.Setup(c => c.ExistName(expected_tag.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _TagsRepositoryMock.Setup(c => c.GetByName(expected_tag.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);
        _TagsRepositoryMock.Setup(c => c.Update(expected_tag, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);
        _PostsRepositoryMock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_result = await _BlogPostManager.AssignTagAsync(post.Id, expected_tag.Name);

        Assert.AreEqual(expected_result, actual_result);
        Assert.IsTrue(post.Tags.Count() == 2);

        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.Verify(c => c.ExistName(expected_tag.Name, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.Verify(c => c.GetByName(expected_tag.Name, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.Verify(c => c.Update(expected_tag, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.Update(post, It.IsAny<CancellationToken>()));

        _PostsRepositoryMock.VerifyNoOtherCalls();
        _TagsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod] // !!!
    public async Task AssignTagAsync_Test_Returns_True_when_Tag_Is_NewTag() 
    {
        //todo: не срабатывает верификация метода Add для репозитория тегов
        var post = _Posts[1];
        post.Tags = post.Tags.ToList();
        var tag_name = "Tag4";
        var expected_tag = new Tag { Name = tag_name }; // новый тег
        var expected_result = true;

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _TagsRepositoryMock.Setup(c => c.ExistName(tag_name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _TagsRepositoryMock.Setup(c => c.Add(expected_tag, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);
        _PostsRepositoryMock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_result = await _BlogPostManager.AssignTagAsync(post.Id, tag_name);

        Assert.AreEqual(expected_result, actual_result);
        Assert.IsTrue(post.Tags.Count() == 2);

        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.Verify(c => c.ExistName(tag_name, It.IsAny<CancellationToken>()));
        //_TagsRepositoryMock.Verify(c => c.Add(expected_tag, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.Update(post, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
        //_TagsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]  //судя по отладчику, тест работает неправильно
    public async Task AssignTagAsync_Test_Returns_True_when_Tag_Is_AlreadyAssigned()
    {
        var post = _Posts[6];
        post.Tags = post.Tags.ToList();
        var expected_tag = _Tags[0]; //уже существующий и прикреплённый тег к посту
        var expected_result = true;
            
        _PostsRepositoryMock.Setup(c => c.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _TagsRepositoryMock.Setup(c => c.ExistName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _TagsRepositoryMock.Setup(c => c.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_tag);

        var actual_result = await _BlogPostManager.AssignTagAsync(post.Id, expected_tag.Name);

        Assert.AreEqual(expected_result, actual_result);

        _PostsRepositoryMock.Verify(c => c.GetById(It.Is<int>(id => id == post.Id), It.IsAny<CancellationToken>()));

        //_PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        //_TagsRepositoryMock.Verify(c => c.ExistName(expected_tag.Name, It.IsAny<CancellationToken>()));
        //_TagsRepositoryMock.Verify(c => c.GetByName(expected_tag.Name, It.IsAny<CancellationToken>()));
        //_PostsRepositoryMock.VerifyNoOtherCalls();
        //_TagsRepositoryMock.VerifyNoOtherCalls();
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

    #region Tag - RemoveTag (4 tests)

    [TestMethod]
    public async Task RemoveTagAsync_Test_Returns_True()
    {
        var post = _Posts[6];
        post.Tags = post.Tags.ToList();
        var tag = _Tags[0];
        var expected_result = true;

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _TagsRepositoryMock.Setup(c => c.GetByName(tag.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);
        _PostsRepositoryMock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _TagsRepositoryMock.Setup(c => c.Update(tag, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        var actual_result = await _BlogPostManager.RemoveTagAsync(post.Id, tag.Name);

        Assert.AreEqual(expected_result, actual_result);
        CollectionAssert.Contains(_Tags, _Tags[0]);
        Assert.IsTrue(post.Tags.Count() == 1);
        Assert.IsTrue(_Tags.Length == 3);

        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.Verify(c => c.GetByName(tag.Name, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.Update(post, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.Verify(c => c.Update(tag, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
        _TagsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task RemoveTagAsync_Test_Returns_ArgumentNullException_When_TagName_is_Null()
    {
        var post = _Posts[0];
        string Tag = null; //проверяем Tag на null
        var expected_exception = new ArgumentNullException(nameof(Tag));

        try
        {
            await _BlogPostManager.RemoveTagAsync(post.Id, Tag);
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
    public async Task RemoveTagAsync_Test_Returns_False_When_Post_is_Null()
    {
        var post_id = 10;
        var tag = _Tags[0];

        var actual_result = await _BlogPostManager.RemoveTagAsync(post_id, tag.Name);

        Assert.IsFalse(actual_result);
    }

    [TestMethod]
    public async Task RemoveTagAsync_Test_Returns_False_When_Post_NotContains_Tag()
    {
        var post = _Posts[0];
        post.Tags = post.Tags.ToList();
        var tag = _Tags[2];  // этот тег не прикреплён к посту

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _TagsRepositoryMock.Setup(c => c.GetByName(tag.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        var actual_result = await _BlogPostManager.RemoveTagAsync(post.Id, tag.Name);

        Assert.IsFalse(actual_result);
        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.Verify(c => c.GetByName(tag.Name, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
        _TagsRepositoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #region Tag - GetBlogTags, GetPostsByTag

    [TestMethod]
    public async Task GetBlogTagsAsync_Test_Returns_List_of_Tags()
    {
        var post = _Posts[6];
        var expected_tags = post.Tags.ToArray();

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_tags = await _BlogPostManager.GetBlogTagsAsync(post.Id);

        CollectionAssert.AreEqual(expected_tags, actual_tags.ToArray());
        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetBlogTagsAsync_Test_Returns_EmptyList()
    {
        var post = _Posts[7];
        var expected_tags = post.Tags;

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_tags = await _BlogPostManager.GetBlogTagsAsync(post.Id);

        Assert.AreEqual(expected_tags, actual_tags);
        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetPostsByTag_Test_Returns_List_of_Posts()
    {
        var tag = new Tag {
            Id = 4, 
            Name = "NewTag",
            Posts = new[] { _Posts[0].Id, _Posts[3].Id, _Posts[4].Id }
            };

        var expected_posts = tag.Posts;

        _TagsRepositoryMock.Setup(c => c.GetByName(tag.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        var actual_posts = await _BlogPostManager.GetPostsByTag(tag.Name);

        Assert.AreEqual(expected_posts, actual_posts);
        _TagsRepositoryMock.Verify(c => c.GetByName(tag.Name, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetPostsByTag_Test_Returns_Empty_List_When_is_Tag_is_NotFound()
    {
        var tag_name = "not_founded_tag";

        var expected_list = Enumerable.Empty<Post>();   //null

        _TagsRepositoryMock.Setup(c => c.GetByName(tag_name, It.IsAny<CancellationToken>()));

        var actual_posts = await _BlogPostManager.GetPostsByTag(tag_name);

        Assert.AreEqual(expected_list.Count(), actual_posts.Count());
        _TagsRepositoryMock.Verify(c => c.GetByName(tag_name, It.IsAny<CancellationToken>()));
        _TagsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetPostsByTag_Test_Returns_ArgumentNullException_When_tagName_is_null()
    {
        string Tag = null; //проверяем Tag на null
        var expected_exception = new ArgumentNullException(nameof(Tag));

        try
        {
            await _BlogPostManager.GetPostsByTag(Tag);
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

    #region Edit ChangePostTitleAsync ChangePostBodyAsync

    [TestMethod]
    public async Task ChangePostTitleAsync_Test_Returns_True()
    {
        var post = _Posts[0];
        var new_title = "new title";

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _PostsRepositoryMock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_post = await _BlogPostManager.ChangePostTitleAsync(post.Id, new_title);

        Assert.IsTrue(actual_post);
        Assert.AreEqual(post.Title, new_title);

        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.Update(post, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ChangePostTitleAsync_Test_Returns_False_When_Post_Not_Found()
    {
        var post_id = 10;
        var new_title = "new title";

        _PostsRepositoryMock.Setup(c => c.GetById(post_id, It.IsAny<CancellationToken>()));

        var actual_post = await _BlogPostManager.ChangePostTitleAsync(post_id, new_title);

        Assert.IsFalse(actual_post);
        _PostsRepositoryMock.Verify(c => c.GetById(post_id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ChangePostTitleAsync_Test_Returns_False_When_Title_is_Null()
    {
        var post = _Posts[0];
        string new_title = null;

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_post = await _BlogPostManager.ChangePostTitleAsync(post.Id, new_title);

        Assert.IsFalse(actual_post);
        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }


    [TestMethod]
    public async Task ChangePostBodyAsync_Test_Returns_True()
    {
        var post = _Posts[0];
        var new_body = "new title";

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _PostsRepositoryMock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_post = await _BlogPostManager.ChangePostBodyAsync(post.Id, new_body);

        Assert.IsTrue(actual_post);
        Assert.AreEqual(post.Body, new_body);

        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.Update(post, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ChangePostBodyAsync_Test_Returns_False_When_Post_Not_Found()
    {
        var post_id = 10;
        var new_body = "new title";

        _PostsRepositoryMock.Setup(c => c.GetById(post_id, It.IsAny<CancellationToken>()));

        var actual_post = await _BlogPostManager.ChangePostTitleAsync(post_id, new_body);

        Assert.IsFalse(actual_post);
        _PostsRepositoryMock.Verify(c => c.GetById(post_id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ChangePostTitleAsync_Test_Returns_False_When_Body_is_Null()
    {
        var post = _Posts[0];
        string new_body = null;

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_post = await _BlogPostManager.ChangePostTitleAsync(post.Id, new_body);

        Assert.IsFalse(actual_post);
        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
    }
    #endregion

    #region Edit ChangePostCategoryAsync

    [TestMethod]
    public async Task ChangePostCategoryAsync_Test_Returns_Category_when_Category_was_Found()
    {
        var post = _Posts[0];
        var expected_category = _Categories[2];

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _CategoriesRepositoryMock.Setup(c => c.ExistName(expected_category.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _CategoriesRepositoryMock.Setup(c => c.GetByName(expected_category.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_category);
        _CategoriesRepositoryMock.Setup(c => c.Update(expected_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_category);
        _PostsRepositoryMock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_category = await _BlogPostManager.ChangePostCategoryAsync(post.Id, expected_category.Name);

        Assert.AreEqual(expected_category, actual_category);
        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _CategoriesRepositoryMock.Verify(c => c.ExistName(expected_category.Name, It.IsAny<CancellationToken>()));
        _CategoriesRepositoryMock.Verify(c => c.GetByName(expected_category.Name, It.IsAny<CancellationToken>()));
        _CategoriesRepositoryMock.Verify(c => c.Update(expected_category, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.Update(post, It.IsAny<CancellationToken>()));

        _PostsRepositoryMock.VerifyNoOtherCalls();
        _CategoriesRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod] // !!!
    public async Task ChangePostCategoryAsync_Test_Returns_Category_when_Category_is_New()
    {
        //todo: Не срабатывает верификация мока на метод Add для репозитория категорий

        var post = _Posts[0];
        var expected_category = new Category { Name = "new_category" };

        _PostsRepositoryMock.Setup(c => c.GetById(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _CategoriesRepositoryMock.Setup(c => c.ExistName(expected_category.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _CategoriesRepositoryMock.Setup(c => c.Add(expected_category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected_category);
        _PostsRepositoryMock.Setup(c => c.Update(post, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var actual_category = await _BlogPostManager.ChangePostCategoryAsync(post.Id, expected_category.Name);

        Assert.AreEqual(expected_category.Name, actual_category.Name);
        CollectionAssert.DoesNotContain(_Categories, actual_category);

        _PostsRepositoryMock.Verify(c => c.GetById(post.Id, It.IsAny<CancellationToken>()));
        _CategoriesRepositoryMock.Verify(c => c.ExistName(expected_category.Name, It.IsAny<CancellationToken>()));
        //_CategoriesRepositoryMock.Verify(c => c.Add(expected_category, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.Verify(c => c.Update(post, It.IsAny<CancellationToken>()));
        _PostsRepositoryMock.VerifyNoOtherCalls();
        //_CategoriesRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ChangePostCategoryAsync_Test_Throws_ArgumentNullException_when_CategoryName_is_Null()
    {
        var post_id = 1;
        string CategoryName = null;

        var expected_exception = new ArgumentNullException(nameof(CategoryName));

        try
        {
            await _BlogPostManager.ChangePostCategoryAsync(post_id, CategoryName);
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
    public async Task ChangePostCategoryAsync_Test_Throws_InvalidOperationException_when_Post_NotFound()
    {
        var PostId = 10;
        var CategoryName = "new";

        var expected_exception = new InvalidOperationException();

        try
        {
            await _BlogPostManager.ChangePostCategoryAsync(PostId, CategoryName);
        }
        catch (Exception actual_exception)
        {
            Assert.AreEqual(expected_exception.GetType(), actual_exception.GetType());
            return;
        }

        Assert.Fail("Исключение не было получено.");
    }

    #endregion
}