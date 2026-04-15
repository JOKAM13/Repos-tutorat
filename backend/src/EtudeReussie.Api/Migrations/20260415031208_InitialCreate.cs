using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtudeReussie.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampusCours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Categorie = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Niveau = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    ObjectifsJson = table.Column<string>(type: "TEXT", nullable: false),
                    RessourcesJson = table.Column<string>(type: "TEXT", nullable: false),
                    DureeEstimee = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusCours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampusFilieres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Niveau = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    NotionsJson = table.Column<string>(type: "TEXT", nullable: false),
                    OutilsJson = table.Column<string>(type: "TEXT", nullable: false),
                    ConseilsJson = table.Column<string>(type: "TEXT", nullable: false),
                    RessourcesJson = table.Column<string>(type: "TEXT", nullable: false),
                    Couleur = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Actif = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusFilieres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TutorApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Subjects = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CoveredLevels = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Institutions = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    About = table.Column<string>(type: "text", nullable: true),
                    Mode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Availability = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TutorRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    LastName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    School = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ClassLevel = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Subject = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Need = table.Column<string>(type: "text", nullable: true),
                    Mode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Availability = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampusFiliereCours",
                columns: table => new
                {
                    FiliereId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoursId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusFiliereCours", x => new { x.FiliereId, x.CoursId });
                    table.ForeignKey(
                        name: "FK_CampusFiliereCours_CampusCours_CoursId",
                        column: x => x.CoursId,
                        principalTable: "CampusCours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampusFiliereCours_CampusFilieres_FiliereId",
                        column: x => x.FiliereId,
                        principalTable: "CampusFilieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampusFiliereCours_CoursId",
                table: "CampusFiliereCours",
                column: "CoursId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampusFiliereCours");

            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "TutorApplications");

            migrationBuilder.DropTable(
                name: "TutorRequests");

            migrationBuilder.DropTable(
                name: "CampusCours");

            migrationBuilder.DropTable(
                name: "CampusFilieres");
        }
    }
}
