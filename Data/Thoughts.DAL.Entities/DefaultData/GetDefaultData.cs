namespace Thoughts.DAL.Entities.DefaultData;

public static class GetDefaultData
{
    public static Status[] DefaultStatus() => new Status[]
    {
           new (){ Id=1, Name="Черновик"},
           new (){ Id=2, Name="Опубликовано"},
           new (){ Id=3, Name="Зблакировано"}
    };

    public static Role[] DefaultRole() => new Role[]
    {
          new (){ Id=1, Name="Администратор"},
          new (){ Id=2, Name="Модератор"},
          new (){ Id=3, Name="Автор"},
    };
}
