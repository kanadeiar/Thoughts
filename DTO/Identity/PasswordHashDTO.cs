namespace DTO.Thoughts.Identity;

public class PasswordHashDTO : UserDTO
{
    public string Hash { get; init; } = null!;
}
