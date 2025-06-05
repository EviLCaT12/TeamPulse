using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPulse.Teams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PerformancesInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "departments");

            migrationBuilder.CreateTable(
                name: "departments",
                schema: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                schema: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teams", x => x.id);
                    table.ForeignKey(
                        name: "fk_teams_departments_department_id",
                        column: x => x.department_id,
                        principalSchema: "departments",
                        principalTable: "departments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    managed_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    department_id = table.Column<Guid>(type: "uuid", nullable: true),
                    managed_department_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_departments_department_id",
                        column: x => x.department_id,
                        principalSchema: "departments",
                        principalTable: "departments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_employees_departments_managed_department_id",
                        column: x => x.managed_department_id,
                        principalSchema: "departments",
                        principalTable: "departments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_employees_teams_managed_team_id",
                        column: x => x.managed_team_id,
                        principalSchema: "departments",
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_employees_teams_team_id",
                        column: x => x.team_id,
                        principalSchema: "departments",
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_employees_department_id",
                schema: "departments",
                table: "employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_managed_department_id",
                schema: "departments",
                table: "employees",
                column: "managed_department_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_managed_team_id",
                schema: "departments",
                table: "employees",
                column: "managed_team_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_team_id",
                schema: "departments",
                table: "employees",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_department_id",
                schema: "departments",
                table: "teams",
                column: "department_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employees",
                schema: "departments");

            migrationBuilder.DropTable(
                name: "teams",
                schema: "departments");

            migrationBuilder.DropTable(
                name: "departments",
                schema: "departments");
        }
    }
}
