namespace EtudeReussie.Api.Models;

public class TutorApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subjects { get; set; } = string.Empty;
    public string? CoveredLevels { get; set; }
    public string? Institutions { get; set; }
    public string? About { get; set; }
    public string? Mode { get; set; }
    public string? City { get; set; }
    public string? Availability { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
