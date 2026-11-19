namespace WebApplication4.Data.Dtos;

/// <summary>
/// Данные пользователя.
/// </summary>
public class UserDto
{
    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
}
