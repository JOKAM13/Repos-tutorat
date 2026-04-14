using EtudeReussie.Api.Data;
using Microsoft.EntityFrameworkCore;
using EtudeReussie.Api.Options;
using EtudeReussie.Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("La chaîne de connexion DefaultConnection est manquante.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Étude Réussie API",
        Version = "v1",
        Description = "API pour enregistrer les demandes Trouver un tuteur, Devenir tuteur et les messages de contact."
    });
});

builder.Services.Configure<AdminAccessOptions>(
    builder.Configuration.GetSection(AdminAccessOptions.SectionName));

builder.Services.AddSingleton<AdminSessionStore>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Étude Réussie API v1");
    options.RoutePrefix = "swagger";
});

app.UseCors("frontend");
app.MapControllers();

app.MapGet("/api/health", () => Results.Ok(new
{
    status = "okkkkkk",
    message = "API Étude Réussie opérationnelle"
}));

app.Run();