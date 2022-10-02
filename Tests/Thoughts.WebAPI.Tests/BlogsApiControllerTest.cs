using Microsoft.Extensions.Logging;

using Moq;

using Thoughts.DAL.Entities;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.Services.Data;
using Thoughts.WebAPI.Services;

using Post = Thoughts.Domain.Base.Entities.Post;

namespace Thoughts.WebAPI.Tests
{
    public class BlogsApiControllerTest
    {
        [Fact]
        public async Task GetAllPosts_OnSuccessStatusCode200()
        {
            //Arrange
            var mockPosts = TestData._Posts;
            var mock = new Mock<IRepository<Domain.Base.Entities.Post>>();
            mock.Setup(services =>
                    services.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockPosts);
            var sut = new BlogsApiController(new BlogPostManager(mock.Object), new Logger<BlogsApiController>(new LoggerFactory()));

            //Act
            var result = (OkObjectResult)await sut.GetAllPosts(CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllPosts_OnSuccess_ReturnPosts()
        {
            //Arrange
            var mockPosts = TestData._Posts;
            var mock = new Mock<IRepository<Post>>();
            mock.Setup(services =>
                    services.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockPosts);
            var sut = new BlogsApiController(new BlogPostManager(mock.Object), new Logger<BlogsApiController>(new LoggerFactory()));

            //Act
            var result = await sut.GetAllPosts(CancellationToken.None);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().Be(mockPosts);
        }

        [Fact]
        public async Task GetAllPosts_NotFound()
        {
            //Arrange
            var mock = new Mock<IRepository<Domain.Base.Entities.Post>>();
            mock.Setup(services =>
                    services.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Post>());
            var sut = new BlogsApiController(new BlogPostManager(mock.Object), new Logger<BlogsApiController>(new LoggerFactory()));

            //Act
            var result = (NotFoundResult)await sut.GetAllPosts(CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllPostCount_OnSuccessStatusCode200()
        {
            //Arrange
            var mockPosts = TestData._Posts;
            var mock = new Mock<IRepository<Domain.Base.Entities.Post>>();
            mock.Setup(services =>
                    services.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockPosts);
            var sut = new BlogsApiController(new BlogPostManager(mock.Object), new Logger<BlogsApiController>(new LoggerFactory()));

            //Act
            var result = (OkObjectResult)await sut.GetAllPostCount(CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(200);
        }
    }
}