using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPulse.Performances.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "performances");

            migrationBuilder.CreateTable(
                name: "group_of_skills",
                schema: "performances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_description = table.Column<string>(type: "text", nullable: false),
                    group_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_of_skills", x => x.id);
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
                name: "skills",
                schema: "performances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_skills", x => x.id);
                    table.ForeignKey(
                        name: "fk_skills_skill_grade_grade_id",
                        column: x => x.grade_id,
                        principalSchema: "performances",
                        principalTable: "skill_grade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_skill",
                schema: "performances",
                columns: table => new
                {
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    skill_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_skill", x => new { x.group_id, x.skill_id });
                    table.ForeignKey(
                        name: "fk_group_skill_group_of_skills_group_id",
                        column: x => x.group_id,
                        principalSchema: "performances",
                        principalTable: "group_of_skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_group_skill_skills_skill_id",
                        column: x => x.skill_id,
                        principalSchema: "performances",
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "record_skills",
                schema: "performances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    record_id = table.Column<Guid>(type: "uuid", nullable: false),
                    self_grade = table.Column<string>(type: "text", nullable: true),
                    manager_grade = table.Column<string>(type: "text", nullable: true),
                    skill_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_record_skills", x => x.id);
                    table.ForeignKey(
                        name: "fk_record_skills_skills_skill_id",
                        column: x => x.skill_id,
                        principalSchema: "performances",
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_group_skill_skill_id",
                schema: "performances",
                table: "group_skill",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_record_skills_skill_id",
                schema: "performances",
                table: "record_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_skills_grade_id",
                schema: "performances",
                table: "skills",
                column: "grade_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_skill",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "record_skills",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "group_of_skills",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "skills",
                schema: "performances");

            migrationBuilder.DropTable(
                name: "skill_grade",
                schema: "performances");
        }
    }
}
