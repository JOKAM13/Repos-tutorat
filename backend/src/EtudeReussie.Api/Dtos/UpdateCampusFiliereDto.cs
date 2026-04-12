using System.ComponentModel.DataAnnotations;

namespace EtudeReussie.Api.Dtos;

public class UpdateCampusFiliereDto
{
    [Required]
    [MaxLength(180)]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Niveau { get; set; } = string.Empty;

    [Required]
    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    public List<string> Notions { get; set; } = new();
    public List<string> Outils { get; set; } = new();
    public List<string> Conseils { get; set; } = new();
    public List<CampusResourceDto> Ressources { get; set; } = new();

    [MaxLength(30)]
    public string Couleur { get; set; } = "#2d6cdf";

    public bool Actif { get; set; } = true;
}
