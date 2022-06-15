using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Services.InSQL;

namespace Thoughts.Services.Tests.InSQL;

[TestClass]
public class DbRepositoryTests
{
    [TestMethod]
    public async Task Get_Returns_Items()
    {
        var logger_mock = new Mock<ILogger<DbRepository<Tag>>>();

        var options = new DbContextOptionsBuilder<ThoughtsDB>()
           .UseInMemoryDatabase("TestDB")
           .Options;

        await using var db = new ThoughtsDB(options);

        var tags = Enumerable.Range(1, 10)
           .Select(i => new Tag { Name = $"Tag-{i}" })
           .ToArray();

        const int skip = 5;
        const int count = 2;
        var expected_tags = tags.Skip(skip).Take(count).ToArray();

        db.Tags.AddRange(tags);
        await db.SaveChangesAsync();

        var tags_repository = new DbRepository<Tag>(db, logger_mock.Object);

        var actual_tags = await tags_repository.Get(skip, count);

        CollectionAssert.AreEqual(expected_tags, actual_tags.ToArray());
    }
}
