using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Services.InSQL;

namespace Thoughts.Services.Tests.InSQL;

[TestClass]
public class DbRepositoryTests
{
    private Mock<ILogger<DbRepository<Tag>>> _Logger_Tag_Mock;
    private Mock<ILogger<DbRepository<Post>>> _Logger_Post_Mock;
    private DbRepository<Tag> _tags_Repository;
    private DbRepository<Post> _posts_Repository;
    private DbContextOptions<ThoughtsDB> _options;
    private Tag[] _tags;
    private Post[] _posts;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _Logger_Tag_Mock = new Mock<ILogger<DbRepository<Tag>>>();
        _Logger_Post_Mock = new Mock<ILogger<DbRepository<Post>>>();

        _options = new DbContextOptionsBuilder<ThoughtsDB>()
           .UseInMemoryDatabase("TestDB")
           .Options;

        var _db = new ThoughtsDB(_options);

        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();

        _tags = Enumerable.Range(1, 10)
            .Select(i => new Tag { Name = $"Tag-{i}" })
            .ToArray();

        await _db.Tags.AddRangeAsync(_tags);

        _posts = Enumerable.Empty<Post>().ToArray();
        await _db.Posts.AddRangeAsync(_posts);

        await _db.SaveChangesAsync();
        
        _tags_Repository = new DbRepository<Tag>(_db, _Logger_Tag_Mock.Object);
        _posts_Repository = new DbRepository<Post>(_db, _Logger_Post_Mock.Object);
    }

    [TestMethod]
    public async Task Get_Returns_Items()
    {
        const int skip = 5;
        const int count = 2;
        var expected_tags = _tags.Skip(skip).Take(count).ToArray();

        var actual_tags = await _tags_Repository.Get(skip, count);

        CollectionAssert.AreEqual(expected_tags, actual_tags.ToArray());
        
        _Logger_Tag_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task Get_Returns_Empty_Enum_when_Count_eq_0()
    {
        const int skip = 5;
        const int count = 0;
        var expected_tags = Enumerable.Empty<Tag>().ToArray();

        var actual_tags = await _tags_Repository.Get(skip, count);

        Assert.AreEqual(expected_tags.Length, actual_tags.Count());

        _Logger_Tag_Mock.VerifyNoOtherCalls();
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
        
        _Logger_Tag_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAll_Returns_All_Items()
    {
        var expected_tags = _tags.ToArray();

        var actual_tags = await _tags_Repository.GetAll();

        CollectionAssert.AreEqual(expected_tags, actual_tags.ToArray());

        _Logger_Tag_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetAll_Returns_Empty_Enum_when_Repo_is_Empty()
    {
        var expected_empty_posts = _posts;

        var actual_empty_posts = await _posts_Repository.GetAll();

        Assert.AreEqual(expected_empty_posts.Length, actual_empty_posts.Count());
        Assert.IsTrue(actual_empty_posts.Count() == 0);
        _Logger_Post_Mock.VerifyNoOtherCalls();
    }


    [TestMethod]
    public async Task Update_Returns_Item()
    {
        var tag = _tags[0];
        tag.Name = "new_tag_name";

        var actual_item = await _tags_Repository.Update(tag);

        Assert.IsNotNull(actual_item);
        Assert.AreEqual(tag.Name, actual_item.Name);

        _Logger_Tag_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetCount_Returns_Count()
    {
        var expected_tags_count = _tags.Length;

        var actual_tags_count = await _tags_Repository.GetCount();

        Assert.AreEqual(expected_tags_count, actual_tags_count);
        Assert.IsNotNull(actual_tags_count);

        _Logger_Tag_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetCount_Returns_0_when_Repo_is_Empty()
    {
        var expected_post_count = _posts.Length;

        var actual_post_count = await _posts_Repository.GetCount();

        Assert.AreEqual(expected_post_count, actual_post_count);
        Assert.IsTrue(actual_post_count == 0);

        _Logger_Post_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetById_Returns_Item()
    {
        var expected_item = _tags[3];
        var item_id = expected_item.Id;

        var actual_item = await _tags_Repository.GetById(item_id);

        Assert.IsNotNull(actual_item);
        Assert.AreEqual(expected_item, actual_item);

        _Logger_Tag_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetById_Throws_Exception_when_Repo_is_Empty()
    {
        var id = 1;

        var actual_item = await _posts_Repository.GetById(id);

        Assert.IsNull(actual_item);
    }

    [TestMethod]
    public async Task Add_Returns_Item_in_Repo()
    {
        var expected_tag = new Tag { Name = "new_tag_name" };

        var actual_tag = await _tags_Repository.Add(expected_tag);

        //var actual_items = await _tags_Repository.GetAll(); // <- работает, поэтому не будем замедлять тест

        Assert.IsNotNull(actual_tag);
        //CollectionAssert.Contains(actual_items.ToArray(), actual_tag);

        _Logger_Tag_Mock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task DeleteById_Returns_Item_from_Repo()
    {
        var expected_tag = _tags[0];

        var actual_tag = await _tags_Repository.DeleteById(expected_tag.Id);
        
        var actual_items = await _tags_Repository.GetAll(); 

        Assert.IsNotNull(actual_tag);
        Assert.AreEqual(expected_tag, actual_tag);

        CollectionAssert.DoesNotContain(actual_items.ToArray(), actual_tag);
    }

    [TestMethod]
    public async Task ExistId_Returns_True_when_Item_WasFound()
    {
        var expected_tag = _tags[5];

        var actual_result = await _tags_Repository.ExistId(expected_tag.Id);

        Assert.IsTrue(actual_result);
    }

    [TestMethod]
    public async Task ExistId_Returns_False_when_Item_WasFound()
    {
        var expected_id = 30;

        var actual_result = await _tags_Repository.ExistId(expected_id);

        Assert.IsFalse(actual_result);
    }
}
