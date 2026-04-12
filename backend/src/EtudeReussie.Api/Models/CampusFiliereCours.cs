namespace EtudeReussie.Api.Models;

public class CampusFiliereCours
{
    public Guid FiliereId { get; set; }
    public CampusFiliere Filiere { get; set; } = null!;

    public Guid CoursId { get; set; }
    public CampusCours Cours { get; set; } = null!;
}
