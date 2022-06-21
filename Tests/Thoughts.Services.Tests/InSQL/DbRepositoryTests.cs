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
    {
        _Logger_Mock = new Mock<ILogger<DbRepository<Tag>>>();
        
        _options = new DbContextOptionsBuilder<ThoughtsDB>()
           .UseInMemoryDatabase("TestDB")
           .Options;

        var _db = new ThoughtsDB(_options);

        _tags = Enumerable.Range(1, 10)
           .Select(i => new Tag { Name = $"Tag-{i}" })
           .ToArray();

        await _db.Tags.AddRangeAsync(_tags);
        await _db.SaveChangesAsync();

        _tags_Repository = new DbRepository<Tag>(_db, _Logger_Mock.Object);
    }

    [TestMethod]
    public async Task Get_Returns_Items()
    {
        const int skip = 5;
        const int count = 2;
        var expected_tags = _tags.Skip(skip).Take(count).ToArray();

        var actual_tags = await _tags_Repository.Get(skip, count);

        CollectionAssert.AreEqual(expected_tags, actual_tags.ToArray());
    }

    [TestMethod]
    public async Task GetPage_Returns_Page()
    {
        const int pageNumber = 2;
        const int pageSize = 3;
        var count = _tags.Length;
        var tags = _tags.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
        var expected_page = new Page<Tag>(tags, pageNumber, pageSize, count);

        var actual_page = await _tags_Repository.GetPage(pageNumber, pageSize);

        CollectionAssert.AreEqual(expected_page.Items.ToArray(), actual_page.Items.ToArray());
    }
}
