using Moq;

using Thoughts.Domain.Base.Entities;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.Services.Tests;

[TestClass]
public class RepositoryBlogPostManagerTests
{

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
