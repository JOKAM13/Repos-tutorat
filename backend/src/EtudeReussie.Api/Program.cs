using EtudeReussie.Api.Data;
using Microsoft.EntityFrameworkCore;
using EtudeReussie.Api.Options;
using EtudeReussie.Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=App_Data/etude-reussie-dev.db";

if (builder.Environment.IsProduction() && connectionString.Contains("dev", StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException("La production ne doit pas utiliser la base de donnees de developpement.");
}

if (builder.Environment.IsDevelopment() && connectionString.Contains("prod", StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException("Le developpement ne doit pas utiliser la base de donnees de production.");
}

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
builder.Services.Configure<AdminAccessOptions>(builder.Configuration.GetSection(AdminAccessOptions.SectionName));
builder.Services.AddSingleton<AdminSessionStore>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://localhost:4300",
                "https://localhost:4200",
                "https://localhost:4300")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "App_Data"));

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Étude Réussie API v1");
        options.RoutePrefix = "swagger";
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("frontend");
app.MapControllers();

app.MapGet("/api/health", () => Results.Ok(new { status = "ok", message = "API Étude Réussie opérationnelle" }));

app.Run();
