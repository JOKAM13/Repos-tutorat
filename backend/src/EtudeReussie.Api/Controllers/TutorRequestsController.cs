using EtudeReussie.Api.Data;
using EtudeReussie.Api.Dtos;
using EtudeReussie.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace EtudeReussie.Api.Controllers;

[ApiController]
[Route("api/tutor-requests")]
public class TutorRequestsController(AppDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTutorRequestDto dto, CancellationToken cancellationToken)
    {
        var entity = new TutorRequest
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone?.Trim(),
            School = dto.School?.Trim(),
            ClassLevel = dto.ClassLevel.Trim(),
            Subject = dto.Subject?.Trim(),
            Need = dto.Need?.Trim(),
            Mode = dto.Mode?.Trim(),
            Availability = dto.Availability?.Trim(),
            City = dto.City?.Trim()
        };

        dbContext.TutorRequests.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Created($"/api/tutor-requests/{entity.Id}", new { entity.Id, message = "Demande enregistrée" });
    }
}
