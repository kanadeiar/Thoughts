namespace Thoughts.DAL.Entities.DefaultData;

public static class GetDefaultData
{
    //public static Status[] DefaultStatus() => new Status[]
    //{
    //    new () { Id = 1, Name = "Черновик" },
    //    new () { Id = 2, Name = "Опубликовано" },
    //    new () { Id = 3, Name = "На модерации" },
    //    new () { Id = 4, Name = "Заблокировано"},
    //};

    public static Role[] DefaultRole() => new Role[]
    {
        new () { Name = "Администратор" },
        new () { Name = "Модератор" },
        new () { Name = "Автор" },
        new () { Name = "Гость" },
    };
}
