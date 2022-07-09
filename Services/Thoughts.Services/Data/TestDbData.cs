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
                FirstName = "Админ",
                LastName = "Админович",
                Patronymic = "Админов",
                NickName="Admin"
            },
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
                Title = "Заголовок 1",
                Body = "<h4>Тело 1.</h4><img src=\"https://catholicsar.ru/wp-content/uploads/1516116812_ryzhiy-maine-coon.jpg\" alt=\"картинка\"/> Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[1], tags[0] },
                Category = categories[0],
                User = users[0],
            },
            new Post
            {
                Title = "Заголовок 2",
                Body = "Тело 2. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[1] },
                Category = categories[0],
                User = users[0],
            },
            new Post
            {
                Title = "Заголовок 3",
                Body = "Тело 3. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[1],
            },
            new Post
            {
                Title = "Заголовок 4",
                Body = "Тело 4. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0] },
                Category = categories[2],
                User = users[2],
            },
            new Post
            {
                Title = "Заголовок 5",
                Body = "Тело 5. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[1] },
                Category = categories[2],
                User = users[2],
            },
            new Post
            {
                Title = "Заголовок 6",
                Body = "Тело 6. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. "
                    + "Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam."
                    + "<br/><img src=\"https://catholicsar.ru/wp-content/uploads/1516116812_ryzhiy-maine-coon.jpg\" alt=\"картинка\"/>",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[2],
            },
            new Post
            {
                Title = "Заголовок 7",
                Body = "Тело 7. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0], tags[2] },
                Category = categories[1],
                User = users[0],
            },
            new Post
            {
                Title = "Заголовок 8",
                Body = "Тело 8. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[2],
            },
            new Post
            {
                Title = "Заголовок 9",
                Body = "Тело 9. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0], tags[2] },
                Category = categories[1],
                User = users[0],
            },
            new Post
            {
                Title = "Заголовок 10",
                Body = "Тело 10. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[2] },
                Category = categories[0],
                User = users[2],
            },
            new Post
            {
                Title = "Заголовок 11",
                Body = "Тело 11. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
                Tags = new[] { tags[0], tags[2] },
                Category = categories[1],
                User = users[0],
            },
            new Post
            {
                Title = "Заголовок 12",
                Body = "Тело 12. Aenean ornare velit lacus, ac varius enim lorem ullamcorper dolore. Proin aliquam facilisis ante interdum. Sed nulla amet lorem feugiat tempus aliquam.",
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
