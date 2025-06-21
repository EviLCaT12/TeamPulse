using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPulse.Teams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    head_of_department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    department_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_team_manager = table.Column<bool>(type: "boolean", nullable: false),
                    is_department_manager = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_departments_working_department_id",
                        column: x => x.department_id,
                        principalSchema: "departments",
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                schema: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: true),
                    head_of_team_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teams_employees_head_of_team_id",
                        column: x => x.head_of_team_id,
                        principalSchema: "departments",
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_departments_head_of_department_id",
                schema: "departments",
                table: "departments",
                column: "head_of_department_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_team_id",
                schema: "departments",
                table: "employees",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_working_department_id",
                schema: "departments",
                table: "employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_department_id",
                schema: "departments",
                table: "teams",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_head_of_team_id",
                schema: "departments",
                table: "teams",
                column: "head_of_team_id");

            migrationBuilder.AddForeignKey(
                name: "fk_departments_employees_head_of_department_id",
                schema: "departments",
                table: "departments",
                column: "head_of_department_id",
                principalSchema: "departments",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_employees_teams_team_id",
                schema: "departments",
                table: "employees",
                column: "team_id",
                principalSchema: "departments",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_departments_employees_head_of_department_id",
                schema: "departments",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "fk_teams_employees_head_of_team_id",
                schema: "departments",
                table: "teams");

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
