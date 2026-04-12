using EtudeReussie.Api.Data;
using EtudeReussie.Api.Dtos;
using EtudeReussie.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EtudeReussie.Api.Controllers;

[ApiController]
[Route("api/campus-passerelle")]
public class CampusPasserelleController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet("filieres/actives")]
    public async Task<IActionResult> GetActiveFilieresAsync(CancellationToken cancellationToken)
    {
        var filieres = await dbContext.CampusFilieres
            .AsNoTracking()
            .Include(item => item.CampusFiliereCours)
            .Where(item => item.Actif)
            .OrderBy(item => item.Nom)
            .ToListAsync(cancellationToken);

        return Ok(filieres.Select(MapFiliere));
    }

    [HttpGet("filieres/{id}")]
    public async Task<IActionResult> GetFiliereDetailsAsync(string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var filiere = await dbContext.CampusFilieres
            .AsNoTracking()
            .Include(item => item.CampusFiliereCours)
                .ThenInclude(link => link.Cours)
            .FirstOrDefaultAsync(item => item.Id == parsedId && item.Actif, cancellationToken);

        if (filiere is null)
        {
            return NotFound(new { message = "Filière introuvable." });
        }

        return Ok(new
        {
            filiere = MapFiliere(filiere),
            cours = filiere.CampusFiliereCours
                .OrderBy(item => item.Cours.Nom)
                .Select(item => MapCours(item.Cours))
        });
    }

    private static object MapFiliere(CampusFiliere item)
    {
        return new
        {
            id = item.Id,
            nom = item.Nom,
            niveau = item.Niveau,
            description = item.Description,
            notions = DeserializeList<string>(item.NotionsJson),
            outils = DeserializeList<string>(item.OutilsJson),
            conseils = DeserializeList<string>(item.ConseilsJson),
            ressources = DeserializeList<CampusResourceDto>(item.RessourcesJson),
            couleur = item.Couleur,
            actif = item.Actif,
            coursIds = item.CampusFiliereCours.Select(link => link.CoursId).ToList()
        };
    }

    private static object MapCours(CampusCours item)
    {
        return new
        {
            id = item.Id,
            nom = item.Nom,
            categorie = item.Categorie,
            niveau = item.Niveau,
            description = item.Description,
            objectifs = DeserializeList<string>(item.ObjectifsJson),
            ressources = DeserializeList<CampusResourceDto>(item.RessourcesJson),
            dureeEstimee = item.DureeEstimee
        };
    }

    private static string SerializeList<T>(IEnumerable<T>? values)
        => JsonSerializer.Serialize(values ?? Array.Empty<T>());

    private static List<T> DeserializeList<T>(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<T>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        catch
        {
            return new List<T>();
        }
    }
}
