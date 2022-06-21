using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Services.InSQL;

namespace Thoughts.Services.Tests.InSQL;

[TestClass]
public class DbRepositoryTests
{
    private Mock<ILogger<DbRepository<Tag>>> _Logger_Mock;
    private DbRepository<Tag> _tags_Repository;
    private DbContextOptions<ThoughtsDB> _options;
    private Tag[] _tags;

    [TestInitialize]
    public async Task TestInitialize()
    //public void TestInitialize()
    {
        _Logger_Mock = new Mock<ILogger<DbRepository<Tag>>>();
        
        _options = new DbContextOptionsBuilder<ThoughtsDB>()
           .UseInMemoryDatabase("TestDB")
           .Options;

        await using var _db = new ThoughtsDB(_options);
        //using var _db = new ThoughtsDB(_options);

        _tags = Enumerable.Range(1, 10)
           .Select(i => new Tag { Name = $"Tag-{i}" })
           .ToArray();

        await _db.Tags.AddRangeAsync(_tags);
        await _db.SaveChangesAsync();

        //_db.AddRange(_tags);
        //_db.SaveChanges();

        _tags_Repository = new DbRepository<Tag>(_db, _Logger_Mock.Object);
    }

    [TestMethod]
    public async Task Get_Returns_Items()
    {
        //var logger_mock = new Mock<ILogger<DbRepository<Tag>>>();

        //var options = new DbContextOptionsBuilder<ThoughtsDB>()
        //   .UseInMemoryDatabase("TestDB")
        //   .Options;

        //await using var db = new ThoughtsDB(options);

        //var tags = Enumerable.Range(1, 10)
        //   .Select(i => new Tag { Name = $"Tag-{i}" })
        //   .ToArray();
        
        const int skip = 5;
        const int count = 2;
        var expected_tags = _tags.Skip(skip).Take(count).ToArray();

        //_db.Tags.AddRange(tags);
        //await _db.SaveChangesAsync();

        //var tags_repository = new DbRepository<Tag>(db, logger_mock.Object);

        var actual_tags = await _tags_Repository.Get(skip, count);

        CollectionAssert.AreEqual(expected_tags, actual_tags.ToArray());
    }
}
