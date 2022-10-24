using System.Security.Claims;

namespace DTO.Thoughts.Identity;

public class ClaimDTO : UserDTO
{
    public IEnumerable<Claim> Claims { get; init; } = null!;
}
