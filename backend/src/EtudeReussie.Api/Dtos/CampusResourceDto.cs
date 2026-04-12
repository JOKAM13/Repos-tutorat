namespace EtudeReussie.Api.Dtos;

public class CampusResourceDto
{
    public string Titre { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Lien { get; set; }
    public string? Description { get; set; }
}
