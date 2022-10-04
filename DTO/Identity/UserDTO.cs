using Thoughts.DAL.Entities.Idetity;

namespace DTO.Identity;

public abstract class UserDTO
{
    public User User { get; init; } = null!;
}
