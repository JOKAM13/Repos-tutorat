using System.ComponentModel.DataAnnotations;

namespace EtudeReussie.Api.Dtos;

public class CreateTutorApplicationDto
{
    [Required]
    [MaxLength(180)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(40)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(500)]
    public string Subjects { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? CoveredLevels { get; set; }

    [MaxLength(250)]
    public string? Institutions { get; set; }

    [MaxLength(4000)]
    public string? About { get; set; }

    [MaxLength(60)]
    public string? Mode { get; set; }

    [MaxLength(120)]
    public string? City { get; set; }

    [MaxLength(200)]
    public string? Availability { get; set; }
}
