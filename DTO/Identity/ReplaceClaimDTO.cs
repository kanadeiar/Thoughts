using System.Security.Claims;

namespace DTO.Identity;

public class ReplaceClaimDTO : UserDTO
{
    public Claim Claim { get; init; } = null!;

    public Claim NewClaim { get; init; } = null!;
}
