using Microsoft.Extensions.Logging;

using Moq;

using Thoughts.DAL.Entities;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.Services.Data;
using Thoughts.WebAPI.Services;

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
            mock.Setup(repo => 
                repo.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockPosts);
            var sut = new BlogsApiController(new BlogPostManager(mock.Object), new Logger<BlogsApiController>(new LoggerFactory()));

            //Act
            var result = (OkObjectResult)await sut.GetAllPosts(CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(200);
        }        
        
        [Fact]
        public async Task GetById_OnSuccessStatusCode200()
        {
            //Arrange
            var mockPosts = TestData._Posts;
            var mock = new Mock<IRepository<Domain.Base.Entities.Post>>();
            mock.Setup(repo =>
                    repo.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(mockPosts);

            var sut = new BlogsApiController(new BlogPostManager(mock.Object), new Logger<BlogsApiController>(new LoggerFactory()));

            //Act
            var count = TestData._Posts.Length;
            var result = (OkObjectResult)await sut.GetById(new Random().Next(1,count), CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(200);
        }        
        
        [Fact]
        public async Task GetAllPostsSkip_OnSuccessStatusCode200()
        {
            //Arrange
            //var sut = new BlogsApiController();
            //
            ////Act
            //var result = (OkObjectResult)await sut.GetAllPostsSkip(CancellationToken.None);
            //
            ////Assert
            //result.StatusCode.Should().Be(200);
        }        
        
        [Fact]
        public async Task GetAllPostCount_OnSuccessStatusCode200()
        {
            //Arrange
            //var sut = new BlogsApiController();
            //
            ////Act
            //var result = (OkObjectResult)await sut.GetAllPostCount(CancellationToken.None);
            //
            ////Assert
            //result.StatusCode.Should().Be(200);
        }        
        
        [Fact]
        public async Task GetAllPostsPage_OnSuccessStatusCode200()
        {
            //Arrange
            //var sut = new BlogsApiController();
            //
            ////Act
            //var result = (OkObjectResult)await sut.GetAllPostsPage(CancellationToken.None);
            //
            ////Assert
            //result.StatusCode.Should().Be(200);
        }
    }
}