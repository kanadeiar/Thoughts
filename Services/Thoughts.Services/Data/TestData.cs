using Thoughts.DAL.Entities;

namespace Thoughts.Services.Data;

public class TestData
{
    public static ICollection<User> Users { get; } = new List<User>
    {
        new() { Id = 1, NickName = "User1", FirstName="User1", LastName="User1" },
        new() { Id = 2, NickName = "User2", FirstName="User2", LastName="User2" },
        new() { Id = 3, NickName = "User3", FirstName="User3", LastName="User3" },
    };

    public static ICollection<Post> Posts { get; } = new List<Post>
    {
        new() { Id = 1, Title = "Post1", Body = "Post1", UserId = 1, /* CategoryId = 1 */ },
        new() { Id = 2, Title = "Post2", Body = "Post2", UserId = 1, /* CategoryId = 2 */ },
        new() { Id = 3, Title = "Post3", Body = "Post3", UserId = 2, /* CategoryId = 1 */ },
        new() { Id = 4, Title = "Post4", Body = "Post4", UserId = 2, /* CategoryId = 3 */ },
        new() { Id = 5, Title = "Post5", Body = "Post5", UserId = 3, /* CategoryId = 2 */ },
        new() { Id = 6, Title = "Post6", Body = "Post6", UserId = 3, /* CategoryId = 3 */ },

    };

    public static ICollection<Category> Categories { get; } = new List<Category>
    {
        new() { Id = 1, Name = "Category1", StatusId = 1 },
        new() { Id = 2, Name = "Category2", StatusId = 2 },
        new() { Id = 3, Name = "Category3", StatusId = 1 },
    };
}
