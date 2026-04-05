using System.ComponentModel.DataAnnotations;

namespace EtudeReussie.Api.Dtos;

public class CreateContactMessageDto
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
    [MaxLength(4000)]
    public string Message { get; set; } = string.Empty;
}
