using Thoughts.DAL.Entities.Idetity;

namespace DTO.Thoughts.Identity;

public abstract class UserDTO
{
    public IdentUser User { get; init; } = null!;
}
