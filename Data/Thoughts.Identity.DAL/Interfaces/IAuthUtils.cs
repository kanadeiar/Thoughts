namespace Thoughts.Identity.DAL.Interfaces
{
    public interface IAuthUtils<TUser>
    {
        string CreateSessionToken(TUser user, IList<string> role);
    }
}
