using EtudeReussie.Api.Data;
using EtudeReussie.Api.Dtos;
using EtudeReussie.Api.Options;
using EtudeReussie.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace EtudeReussie.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private const string AdminTokenHeader = "X-Admin-Token";

    private readonly AppDbContext _dbContext;
    private readonly AdminAccessOptions _adminAccessOptions;
    private readonly AdminSessionStore _adminSessionStore;

    public AdminController(
        AppDbContext dbContext,
        IOptions<AdminAccessOptions> adminAccessOptions,
        AdminSessionStore adminSessionStore)
    {
        _dbContext = dbContext;
        _adminAccessOptions = adminAccessOptions.Value;
        _adminSessionStore = adminSessionStore;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] AdminLoginRequestDto dto)
    {
        var username = dto.Username?.Trim() ?? string.Empty;
        var password = dto.Password ?? string.Empty;

        if (!string.Equals(username, _adminAccessOptions.Username, StringComparison.Ordinal) ||
            !string.Equals(password, _adminAccessOptions.Password, StringComparison.Ordinal))
        {
            return Unauthorized(new { message = "Identifiants administrateur invalides." });
        }

        var token = _adminSessionStore.CreateToken();
        return Ok(new { token, username = _adminAccessOptions.Username });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var token = Request.Headers[AdminTokenHeader].FirstOrDefault();
        _adminSessionStore.Revoke(token);
        return Ok(new { message = "Déconnexion effectuée." });
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardAsync(CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        var tutorRequests = await _dbContext.TutorRequests
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var tutorApplications = await _dbContext.TutorApplications
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var contactMessages = await _dbContext.ContactMessages
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            tutorRequests,
            tutorApplications,
            contactMessages
        });
    }


    [HttpDelete("tutor-requests/{id}")]
    public async Task<IActionResult> DeleteTutorRequestAsync(string id, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var item = await _dbContext.TutorRequests.FirstOrDefaultAsync(x => x.Id == parsedId, cancellationToken);
        if (item is null)
        {
            return NotFound(new { message = "Demande introuvable." });
        }

        _dbContext.TutorRequests.Remove(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Demande supprimée." });
    }

    [HttpDelete("tutor-applications/{id}")]
    public async Task<IActionResult> DeleteTutorApplicationAsync(string id, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var item = await _dbContext.TutorApplications.FirstOrDefaultAsync(x => x.Id == parsedId, cancellationToken);
        if (item is null)
        {
            return NotFound(new { message = "Candidature introuvable." });
        }

        _dbContext.TutorApplications.Remove(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Candidature supprimée." });
    }

    [HttpDelete("contact-messages/{id}")]
    public async Task<IActionResult> DeleteContactMessageAsync(string id, CancellationToken cancellationToken)
    {
        if (!IsAuthorized())
        {
            return Unauthorized(new { message = "Accès administrateur requis." });
        }

        if (!Guid.TryParse(id, out var parsedId))
        {
            return BadRequest(new { message = "Identifiant invalide." });
        }

        var item = await _dbContext.ContactMessages.FirstOrDefaultAsync(x => x.Id == parsedId, cancellationToken);
        if (item is null)
        {
            return NotFound(new { message = "Message introuvable." });
        }

        _dbContext.ContactMessages.Remove(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Message supprimé." });
    }

    private bool IsAuthorized()
    {
        var token = Request.Headers[AdminTokenHeader].FirstOrDefault();
        return _adminSessionStore.IsValid(token);
    }
}
