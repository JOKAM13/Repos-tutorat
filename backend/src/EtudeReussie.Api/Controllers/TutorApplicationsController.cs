using EtudeReussie.Api.Data;
using EtudeReussie.Api.Dtos;
using EtudeReussie.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace EtudeReussie.Api.Controllers;

[ApiController]
[Route("api/tutor-applications")]
public class TutorApplicationsController(AppDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTutorApplicationDto dto, CancellationToken cancellationToken)
    {
        var entity = new TutorApplication
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone?.Trim(),
            Subjects = dto.Subjects.Trim(),
            CoveredLevels = dto.CoveredLevels?.Trim(),
            Institutions = dto.Institutions?.Trim(),
            About = dto.About?.Trim(),
            Mode = dto.Mode?.Trim(),
            City = dto.City?.Trim(),
            Availability = dto.Availability?.Trim()
        };

        dbContext.TutorApplications.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Created($"/api/tutor-applications/{entity.Id}", new { entity.Id, message = "Candidature enregistrée" });
    }
}
