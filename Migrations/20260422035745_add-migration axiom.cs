using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace axiomtechno.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationaxiom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClTelefono = table.Column<long>(type: "bigint", nullable: false),
                    ClImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClActivo = table.Column<bool>(type: "bit", nullable: false),
                    ClFechaAlta = table.Column<DateOnly>(type: "date", nullable: false),
                    ClFechaBaja = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    PagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagMonto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PagFecha = table.Column<DateOnly>(type: "date", nullable: false),
                    ClId = table.Column<int>(type: "int", nullable: false),
                    ClienteClId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.PagId);
                    table.ForeignKey(
                        name: "FK_Pagos_Clientes_ClienteClId",
                        column: x => x.ClienteClId,
                        principalTable: "Clientes",
                        principalColumn: "ClId");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsDni = table.Column<long>(type: "bigint", nullable: true),
                    UsFechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsFechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsCorreo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsPasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    token_recovery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsActivo = table.Column<bool>(type: "bit", nullable: false),
                    UsTelefono = table.Column<long>(type: "bigint", nullable: false),
                    RolID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsId);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolID",
                        column: x => x.RolID,
                        principalTable: "Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ClienteClId",
                table: "Pagos",
                column: "ClienteClId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolID",
                table: "Usuarios",
                column: "RolID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
