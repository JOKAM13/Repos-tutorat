namespace EtudeReussie.Api.Models;

public class CampusFiliere
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nom { get; set; } = string.Empty;
    public string Niveau { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string NotionsJson { get; set; } = "[]";
    public string OutilsJson { get; set; } = "[]";
    public string ConseilsJson { get; set; } = "[]";
    public string RessourcesJson { get; set; } = "[]";
    public string Couleur { get; set; } = "#2d6cdf";
    public bool Actif { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<CampusFiliereCours> CampusFiliereCours { get; set; } = new List<CampusFiliereCours>();
}
