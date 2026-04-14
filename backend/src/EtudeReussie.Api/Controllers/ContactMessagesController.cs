using EtudeReussie.Api.Data;
using EtudeReussie.Api.Dtos;
using EtudeReussie.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace EtudeReussie.Api.Controllers;

[ApiController]
[Route("api/contact-messages")]
public class ContactMessagesController(AppDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateContactMessageDto dto, CancellationToken cancellationToken)
    {
        var entity = new ContactMessage
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone?.Trim(),
            Message = dto.Message.Trim()
        };

        dbContext.ContactMessages.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Created($"/api/contact-messages/{entity.Id}", new { entity.Id, message = "Message enregistré" });
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { status = "ok", message = "route de test fonctionne" });
    }
}
