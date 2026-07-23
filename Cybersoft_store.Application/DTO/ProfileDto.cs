public class ProfileDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Alias { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Avatar { get; set; }

}