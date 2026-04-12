using System.ComponentModel.DataAnnotations;

namespace EtudeReussie.Api.Dtos;

public class AssignCampusCoursDto
{
    [Required]
    public string FiliereId { get; set; } = string.Empty;

    [Required]
    public string CoursId { get; set; } = string.Empty;
}
