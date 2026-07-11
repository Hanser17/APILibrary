using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Autor_id = table.Column<int>(type: "int", nullable: false),
                    Año_publicacion = table.Column<int>(type: "int", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libros_Autores_Autor_id",
                        column: x => x.Autor_id,
                        principalTable: "Autores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libro_id = table.Column<int>(type: "int", nullable: false),
                    Fecha_prestamo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fecha_devolucion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prestamos_Libros_Libro_id",
                        column: x => x.Libro_id,
                        principalTable: "Libros",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Autor_id",
                table: "Libros",
                column: "Autor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_Libro_id",
                table: "Prestamos",
                column: "Libro_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Autores");
        }
    }
}
