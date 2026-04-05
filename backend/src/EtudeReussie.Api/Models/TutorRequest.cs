namespace EtudeReussie.Api.Models;

public class TutorRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? School { get; set; }
    public string ClassLevel { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string? Need { get; set; }
    public string? Mode { get; set; }
    public string? Availability { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
