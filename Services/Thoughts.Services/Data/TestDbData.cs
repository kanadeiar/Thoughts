using Category = Thoughts.DAL.Entities.Category;
using Post = Thoughts.DAL.Entities.Post;
using Tag = Thoughts.DAL.Entities.Tag;
using User = Thoughts.DAL.Entities.User;

namespace Thoughts.Services.Data;

public class TestDbData
{
    static TestDbData()
    {
        var tags = new Tag[]
        {
            new(){ Name = "Tag1", },
            new(){ Name = "Tag2", },
            new(){ Name = "Tag3", },
        };

        var categories = new Category[]
        {
            new(){ Name = "Category1", },
            new(){ Name = "Category2", },
            new(){ Name = "Category3", },
        };

        var users = new User[]
        {
            new()
            {
                FirstName = "User1",
                LastName = "User1",
                NickName="User1"
            },
            new()
            {
                FirstName = "User2",
                LastName = "User2",
                NickName = "User2"
            },
            new()
            {
                FirstName = "User3",
                LastName = "User3",
                NickName = "User3"
            },
        };

        var posts = new[]
        {
            new Post
            {
                Title = "Title1",
                Body = "Body1. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[1] },
                Category = categories[0],
                User = users[0],
            },
            new Post
            {
                Title = "Title2",
                Body = "Body2. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[1] },
                Category = categories[0],
                User = users[0],
            },
            new Post
            {
                Title = "Title3",
                Body = "Body3. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[1],
            },
            new Post
            {
                Title = "Title4",
                Body = "Body4. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0] },
                Category = categories[2],
                User = users[2],
            },
            new Post
            {
                Title = "Title5",
                Body = "Body5. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[1] },
                Category = categories[2],
                User = users[2],
            },
            new Post
            {
                Title = "Title6",
                Body = "Body6. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[2],
            },
            new Post
            {
                Title = "Title7",
                Body = "Body7. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0], tags[2] },
                Category = categories[1],
                User = users[0],
            },
            new Post
            {
                Title = "Title8",
                Body = "Body8. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[2],
            },
            new Post
            {
                Title = "Title9",
                Body = "Body9. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0], tags[2] },
                Category = categories[1],
                User = users[0],
            },
            new Post
            {
                Title = "Title10",
                Body = "Body10. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[2],
            },
            new Post
            {
                Title = "Title11",
                Body = "Body11. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0], tags[2] },
                Category = categories[1],
                User = users[0],
            },
            new Post
            {
                Title = "Title12",
                Body = "Body12. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[2],
            },
        };

        Tags = tags;
        Categories = categories;
        Users = users;
        Posts = posts;
    }

    public static ICollection<Tag> Tags { get; }

    public static ICollection<Category> Categories { get; }

    public static ICollection<User> Users { get; }


    public static ICollection<Post> Posts { get; }
}
