using System.ComponentModel.DataAnnotations;

namespace EtudeReussie.Api.Dtos;

public class CreateTutorRequestDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(40)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string? School { get; set; }

    [Required]
    [MaxLength(160)]
    public string ClassLevel { get; set; } = string.Empty;

    [MaxLength(160)]
    public string? Subject { get; set; }

    [MaxLength(4000)]
    public string? Need { get; set; }

    [MaxLength(60)]
    public string? Mode { get; set; }

    [MaxLength(200)]
    public string? Availability { get; set; }

    [MaxLength(120)]
    public string? City { get; set; }
}
