namespace EtudeReussie.Api.Models;

public class CampusCours
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nom { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public string Niveau { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ObjectifsJson { get; set; } = "[]";
    public string RessourcesJson { get; set; } = "[]";
    public string DureeEstimee { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<CampusFiliereCours> CampusFiliereCours { get; set; } = new List<CampusFiliereCours>();
}
