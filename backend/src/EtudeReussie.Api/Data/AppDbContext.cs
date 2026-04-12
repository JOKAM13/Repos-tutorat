using EtudeReussie.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EtudeReussie.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TutorRequest> TutorRequests => Set<TutorRequest>();
    public DbSet<TutorApplication> TutorApplications => Set<TutorApplication>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<CampusFiliere> CampusFilieres => Set<CampusFiliere>();
    public DbSet<CampusCours> CampusCours => Set<CampusCours>();
    public DbSet<CampusFiliereCours> CampusFiliereCours => Set<CampusFiliereCours>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TutorRequest>(entity =>
        {
            entity.ToTable("TutorRequests");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.FirstName).HasMaxLength(120).IsRequired();
            entity.Property(item => item.LastName).HasMaxLength(120).IsRequired();
            entity.Property(item => item.Email).HasMaxLength(255).IsRequired();
            entity.Property(item => item.ClassLevel).HasMaxLength(160).IsRequired();
            entity.Property(item => item.Phone).HasMaxLength(40);
            entity.Property(item => item.School).HasMaxLength(200);
            entity.Property(item => item.Subject).HasMaxLength(160);
            entity.Property(item => item.Mode).HasMaxLength(60);
            entity.Property(item => item.Availability).HasMaxLength(200);
            entity.Property(item => item.City).HasMaxLength(120);
            entity.Property(item => item.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<TutorApplication>(entity =>
        {
            entity.ToTable("TutorApplications");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.FullName).HasMaxLength(180).IsRequired();
            entity.Property(item => item.Email).HasMaxLength(255).IsRequired();
            entity.Property(item => item.Phone).HasMaxLength(40);
            entity.Property(item => item.Subjects).HasMaxLength(500).IsRequired();
            entity.Property(item => item.CoveredLevels).HasMaxLength(250);
            entity.Property(item => item.Institutions).HasMaxLength(250);
            entity.Property(item => item.Mode).HasMaxLength(60);
            entity.Property(item => item.City).HasMaxLength(120);
            entity.Property(item => item.Availability).HasMaxLength(200);
            entity.Property(item => item.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.ToTable("ContactMessages");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.FullName).HasMaxLength(180).IsRequired();
            entity.Property(item => item.Email).HasMaxLength(255).IsRequired();
            entity.Property(item => item.Phone).HasMaxLength(40);
            entity.Property(item => item.Message).HasMaxLength(4000).IsRequired();
            entity.Property(item => item.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<CampusFiliere>(entity =>
        {
            entity.ToTable("CampusFilieres");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Nom).HasMaxLength(180).IsRequired();
            entity.Property(item => item.Niveau).HasMaxLength(80).IsRequired();
            entity.Property(item => item.Description).HasMaxLength(4000).IsRequired();
            entity.Property(item => item.NotionsJson).HasColumnType("TEXT").IsRequired();
            entity.Property(item => item.OutilsJson).HasColumnType("TEXT").IsRequired();
            entity.Property(item => item.ConseilsJson).HasColumnType("TEXT").IsRequired();
            entity.Property(item => item.RessourcesJson).HasColumnType("TEXT").IsRequired();
            entity.Property(item => item.Couleur).HasMaxLength(30).IsRequired();
            entity.Property(item => item.Actif).IsRequired();
            entity.Property(item => item.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<CampusCours>(entity =>
        {
            entity.ToTable("CampusCours");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Nom).HasMaxLength(180).IsRequired();
            entity.Property(item => item.Categorie).HasMaxLength(120).IsRequired();
            entity.Property(item => item.Niveau).HasMaxLength(80).IsRequired();
            entity.Property(item => item.Description).HasMaxLength(4000).IsRequired();
            entity.Property(item => item.ObjectifsJson).HasColumnType("TEXT").IsRequired();
            entity.Property(item => item.RessourcesJson).HasColumnType("TEXT").IsRequired();
            entity.Property(item => item.DureeEstimee).HasMaxLength(120).IsRequired();
            entity.Property(item => item.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<CampusFiliereCours>(entity =>
        {
            entity.ToTable("CampusFiliereCours");
            entity.HasKey(item => new { item.FiliereId, item.CoursId });

            entity.HasOne(item => item.Filiere)
                .WithMany(item => item.CampusFiliereCours)
                .HasForeignKey(item => item.FiliereId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.Cours)
                .WithMany(item => item.CampusFiliereCours)
                .HasForeignKey(item => item.CoursId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
