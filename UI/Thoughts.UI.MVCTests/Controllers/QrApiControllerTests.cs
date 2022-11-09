using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Thoughts.UI.MVC.Controllers;
using static Xunit.Assert;

namespace Thoughts.UI.MVCTests.Controllers
{
    [TestClass]
    public class QrApiControllerTests
    {
        private Mock<QrApiController> _controllerMock;
        private Mock<ILogger<QrApiController>> _loggerMock;

        private QrApiController _controller;

        [TestInitialize]
        public void Initialize()
        {
            var logger = new Mock<ILogger<QrApiController>>();
            var controller = new QrApiController(logger.Object);
            _controller = controller;
        }

        [TestCleanup]
        public void Finalize()
        {

        }

        [TestMethod]
        public async Task Get_Positive_Test()
        {
            var testCode = "123456";
            var contentTypeTest = "application/png";
            var fileContentsTest = new byte[]{
                137,80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 1, 34, 0, 0, 1, 34, 1, 0, 0, 0, 0, 117,
               197, 226, 27, 0, 0, 1, 35, 73, 68, 65, 84, 120, 156, 237, 152, 193, 13, 195, 32, 12, 69, 145, 58, 64, 71, 202,
               234, 140, 148, 1, 42, 81, 12, 54, 129, 166, 77, 122, 192, 145, 90, 61, 31, 16, 129, 151, 203, 119, 252, 77, 8, 233,
               139, 136, 1, 10, 37, 248, 38, 168, 14, 124, 226, 47, 188, 48, 104, 220, 100, 150, 135, 101, 181, 149, 5, 202, 91, 137,
               242, 144, 86, 1, 202, 99, 158, 117, 27, 80, 174, 74, 72, 74, 100, 79, 129, 181, 229, 10, 234, 66, 37, 90, 174, 160, 174,
               87, 226, 161, 51, 168, 139, 148, 176, 188, 216, 75, 199, 254, 5, 53, 81, 137, 173, 35, 171, 61, 157, 244, 109, 168, 105, 74,
               108, 209, 219, 83, 11, 40, 79, 37, 162, 157, 63, 109, 86, 156, 233, 53, 67, 80, 14, 74, 148, 133, 20, 239, 210, 16, 202, 194, 103,
               103, 130, 154, 172, 68, 94, 86, 39, 202, 168, 238, 133, 112, 223, 213, 16, 148, 155, 18, 146, 161, 186, 39, 25, 106, 60, 148, 191, 
               18, 205, 143, 236, 63, 216, 138, 5, 202, 85, 9, 11, 93, 206, 185, 170, 195, 88, 29, 80, 243, 149, 24, 78, 162, 230, 81, 173, 53, 
               64, 185, 42, 177, 245, 97, 241, 163, 218, 155, 119, 93, 1, 202, 71, 137, 241, 214, 39, 118, 246, 4, 117, 149, 18, 53, 67, 181, 
               48, 228, 167, 224, 125, 134, 160, 60, 148, 208, 91, 254, 97, 128, 242, 85, 194, 156, 169, 59, 137, 30, 116, 5, 168, 137, 74, 
               180, 142, 220, 221, 60, 244, 70, 5, 229, 166, 196, 73, 64, 161, 4, 223, 4, 213, 129, 79, 164, 95, 247, 194, 39, 17, 7, 84, 
               37, 107, 251, 69, 196, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130
            };
            var getMethod = await _controller.Get(testCode);
            var fileResult = IsType<FileContentResult>(getMethod);
            Equal(contentTypeTest, fileResult.ContentType);
            Equal(fileContentsTest, fileResult.FileContents);

        }
        [TestMethod]
        public async Task Get_Negative_Test()
        {
            var statusCodeTest = 404;
            var getMethod = await _controller.Get("");
            var fileResult = IsType<NotFoundResult>(getMethod);
            Equal(statusCodeTest, fileResult.StatusCode);
        }
    }
}