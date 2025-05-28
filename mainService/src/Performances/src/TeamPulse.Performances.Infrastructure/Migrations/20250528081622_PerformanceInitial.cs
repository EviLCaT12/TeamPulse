using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPulse.Performances.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PerformanceInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "performances");

            migrationBuilder.CreateTable(
                name: "record_skills",
                schema: "performances",
                columns: table => new
                {
                    who_id = table.Column<Guid>(type: "uuid", nullable: false),
                    what_id = table.Column<Guid>(type: "uuid", nullable: false),
                    self_grade = table.Column<string>(type: "text", nullable: true),
                    manager_grade = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_record_skills", x => new { x.who_id, x.what_id });
                });

            migrationBuilder.CreateTable(
                name: "skill_grade",
                schema: "performances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    grades = table.Column<string>(type: "jsonb", nullable: false),
                    grade_type = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    grade_description = table.Column<string>(type: "text", nullable: false),
                    grade_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_skill_grade", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "group_of_skills",
                schema: "performances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_description = table.Column<string>(type: "text", nullable: false),
                    group_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_of_skills", x => x.id);
                    table.ForeignKey(
                        name: "fk_group_of_skills_skill_grade_grade_id",
                        column: x => x.grade_id,
                        principalSchema: "performances",
                        principalTable: "skill_grade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "skills",
                schema: "performances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_of_skills_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_skills", x => x.id);
                    table.ForeignKey(
                        name: "fk_skills_group_of_skills_group_of_skills_id",
                        column: x => x.group_of_skills_id,
                        principalSchema: "performances",
                        principalTable: "group_of_skills",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_skills_skill_grade_grade_id",
                        column: x => x.grade_id,
                        principalSchema: "performances",
                        principalTable: "skill_grade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_skills",
                schema: "performances",
                columns: table => new
                {
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    skill_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_skills", x => new { x.group_id, x.skill_id });
                    table.ForeignKey(
                        name: "fk_group_skills_group_of_skills_group_id",
                        column: x => x.group_id,
                        principalSchema: "performances",
                        principalTable: "group_of_skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_group_skills_skills_skill_id",
                        column: x => x.skill_id,
                        principalSchema: "performances",
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_group_of_skills_grade_id",
                schema: "performances",
                table: "group_of_skills",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "ix_group_skills_skill_id",
                schema: "performances",
                table: "group_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_skills_grade_id",
                schema: "performances",
                table: "skills",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "ix_skills_group_of_skills_id",
                schema: "performances",
                table: "skills",
                column: "group_of_skills_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_skills",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "record_skills",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "skills",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "group_of_skills",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "skill_grade",
                schema: "performances");
        }
    }
}
