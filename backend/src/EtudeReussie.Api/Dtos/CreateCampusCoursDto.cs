using System.ComponentModel.DataAnnotations;

namespace EtudeReussie.Api.Dtos;

public class CreateCampusCoursDto
{
    [Required]
    [MaxLength(180)]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Categorie { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Niveau { get; set; } = string.Empty;

    [Required]
    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    public List<string> Objectifs { get; set; } = new();
    public List<CampusResourceDto> Ressources { get; set; } = new();

    [Required]
    [MaxLength(120)]
    public string DureeEstimee { get; set; } = string.Empty;
}
