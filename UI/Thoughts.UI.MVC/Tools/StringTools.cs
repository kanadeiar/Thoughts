namespace Thoughts.UI.MVC.Tools;

/// <summary>
/// Инструменты для работы со строками
/// </summary>
public class StringTools
{
    /// <summary>
    /// Обрезать тело сообщения до приемлемых величин
    /// </summary>
    /// <param name="posts"></param>
    public static void CutBodyTextInPosts(Domain.Base.Entities.Post[] posts, int length)
    {
        foreach (var e in posts)
        {
            if (e.Body.Length > length)
            {
                e.Body = string.Concat(e.Body[..length], " ...");
            }
        }
    }
}
