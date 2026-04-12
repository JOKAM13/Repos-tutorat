using EtudeReussie.Api.Data;
using EtudeReussie.Api.Dtos;
using EtudeReussie.Api.Models;
using EtudeReussie.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EtudeReussie.Api.Controllers;

[ApiController]
[Route("api/admin/campus-passerelle")]
public class AdminCampusPasserelleController(AppDbContext dbContext, AdminSessionStore adminSessionStore) : ControllerBase
{
    private const string AdminTokenHeader = "X-Admin-Token";

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardAsync(CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        var filieres = await dbContext.CampusFilieres
            .AsNoTracking()
            .Include(item => item.CampusFiliereCours)
            .OrderBy(item => item.Nom)
            .ToListAsync(cancellationToken);

        var cours = await dbContext.CampusCours
            .AsNoTracking()
            .OrderBy(item => item.Nom)
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            filieres = filieres.Select(MapFiliere),
            cours = cours.Select(MapCours)
        });
    }

    [HttpPost("filieres")]
    public async Task<IActionResult> CreateFiliereAsync([FromBody] CreateCampusFiliereDto dto, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        var entity = new CampusFiliere
        {
            Nom = dto.Nom.Trim(),
            Niveau = dto.Niveau.Trim(),
            Description = dto.Description.Trim(),
            NotionsJson = SerializeList(dto.Notions.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item))),
            OutilsJson = SerializeList(dto.Outils.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item))),
            ConseilsJson = SerializeList(dto.Conseils.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item))),
            RessourcesJson = SerializeList(dto.Ressources),
            Couleur = string.IsNullOrWhiteSpace(dto.Couleur) ? "#2d6cdf" : dto.Couleur.Trim(),
            Actif = dto.Actif
        };

        dbContext.CampusFilieres.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        entity.CampusFiliereCours = new List<CampusFiliereCours>();
        return Created($"/api/admin/campus-passerelle/filieres/{entity.Id}", MapFiliere(entity));
    }

    [HttpPut("filieres/{id}")]
    public async Task<IActionResult> UpdateFiliereAsync(string id, [FromBody] UpdateCampusFiliereDto dto, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var entity = await dbContext.CampusFilieres
            .Include(item => item.CampusFiliereCours)
            .FirstOrDefaultAsync(item => item.Id == parsedId, cancellationToken);

        if (entity is null)
        {
            return NotFound(new { message = "Filière introuvable." });
        }

        entity.Nom = dto.Nom.Trim();
        entity.Niveau = dto.Niveau.Trim();
        entity.Description = dto.Description.Trim();
        entity.NotionsJson = SerializeList(dto.Notions.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item)));
        entity.OutilsJson = SerializeList(dto.Outils.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item)));
        entity.ConseilsJson = SerializeList(dto.Conseils.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item)));
        entity.RessourcesJson = SerializeList(dto.Ressources);
        entity.Couleur = string.IsNullOrWhiteSpace(dto.Couleur) ? "#2d6cdf" : dto.Couleur.Trim();
        entity.Actif = dto.Actif;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(MapFiliere(entity));
    }

    [HttpDelete("filieres/{id}")]
    public async Task<IActionResult> DeleteFiliereAsync(string id, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var entity = await dbContext.CampusFilieres.FirstOrDefaultAsync(item => item.Id == parsedId, cancellationToken);
        if (entity is null)
        {
            return NotFound(new { message = "Filière introuvable." });
        }

        dbContext.CampusFilieres.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Filière supprimée." });
    }

    [HttpPost("cours")]
    public async Task<IActionResult> CreateCoursAsync([FromBody] CreateCampusCoursDto dto, CancellationToken cancellationToken)
    {
       /* if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }*/

        var entity = new CampusCours
        {
            Nom = dto.Nom.Trim(),
            Categorie = dto.Categorie.Trim(),
            Niveau = dto.Niveau.Trim(),
            Description = dto.Description.Trim(),
            ObjectifsJson = SerializeList(dto.Objectifs.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item))),
            RessourcesJson = SerializeList(dto.Ressources),
            DureeEstimee = dto.DureeEstimee.Trim()
        };

        dbContext.CampusCours.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Created($"/api/admin/campus-passerelle/cours/{entity.Id}", MapCours(entity));
    }

    [HttpPut("cours/{id}")]
    public async Task<IActionResult> UpdateCoursAsync(string id, [FromBody] UpdateCampusCoursDto dto, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var entity = await dbContext.CampusCours.FirstOrDefaultAsync(item => item.Id == parsedId, cancellationToken);
        if (entity is null)
        {
            return NotFound(new { message = "Cours introuvable." });
        }

        entity.Nom = dto.Nom.Trim();
        entity.Categorie = dto.Categorie.Trim();
        entity.Niveau = dto.Niveau.Trim();
        entity.Description = dto.Description.Trim();
        entity.ObjectifsJson = SerializeList(dto.Objectifs.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item)));
        entity.RessourcesJson = SerializeList(dto.Ressources);
        entity.DureeEstimee = dto.DureeEstimee.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(MapCours(entity));
    }

    [HttpDelete("cours/{id}")]
    public async Task<IActionResult> DeleteCoursAsync(string id, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var entity = await dbContext.CampusCours.FirstOrDefaultAsync(item => item.Id == parsedId, cancellationToken);
        if (entity is null)
        {
            return NotFound(new { message = "Cours introuvable." });
        }

        dbContext.CampusCours.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Cours supprimé." });
    }

    [HttpPost("assignations")]
    public async Task<IActionResult> AssignCoursAsync([FromBody] AssignCampusCoursDto dto, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(dto.FiliereId, out var filiereId) || !Guid.TryParse(dto.CoursId, out var coursId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var filiereExists = await dbContext.CampusFilieres.AnyAsync(item => item.Id == filiereId, cancellationToken);
        var coursExists = await dbContext.CampusCours.AnyAsync(item => item.Id == coursId, cancellationToken);

        if (!filiereExists || !coursExists)
        {
            return NotFound(new { message = "Filière ou cours introuvable." });
        }

        var exists = await dbContext.CampusFiliereCours.AnyAsync(
            item => item.FiliereId == filiereId && item.CoursId == coursId,
            cancellationToken);

        if (!exists)
        {
            dbContext.CampusFiliereCours.Add(new CampusFiliereCours
            {
                FiliereId = filiereId,
                CoursId = coursId
            });

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return Ok(new { message = "Cours assigné à la filière." });
    }

    [HttpDelete("filieres/{filiereId}/cours/{coursId}")]
    public async Task<IActionResult> RemoveCoursAsync(string filiereId, string coursId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(filiereId, out var parsedFiliereId) || !Guid.TryParse(coursId, out var parsedCoursId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var link = await dbContext.CampusFiliereCours.FirstOrDefaultAsync(
            item => item.FiliereId == parsedFiliereId && item.CoursId == parsedCoursId,
            cancellationToken);

        if (link is null)
        {
            return NotFound(new { message = "Assignation introuvable." });
        }

        dbContext.CampusFiliereCours.Remove(link);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Cours retiré de la filière." });
    }

    private bool IsAuthorized()
    {
        var token = Request.Headers[AdminTokenHeader].FirstOrDefault();
        return adminSessionStore.IsValid(token);
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
