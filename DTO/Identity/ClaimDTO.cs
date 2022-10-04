using System.Security.Claims;

namespace DTO.Identity;

public class ClaimDTO : UserDTO
{
    public IEnumerable<Claim> Claims { get; init; } = null!;
}
