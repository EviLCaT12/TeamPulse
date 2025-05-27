using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPulse.Performances.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addskillgradetogroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "grade_id",
                schema: "performances",
                table: "group_of_skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_group_of_skills_grade_id",
                schema: "performances",
                table: "group_of_skills",
                column: "grade_id");

            migrationBuilder.AddForeignKey(
                name: "fk_group_of_skills_skill_grade_grade_id",
                schema: "performances",
                table: "group_of_skills",
                column: "grade_id",
                principalSchema: "performances",
                principalTable: "skill_grade",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_group_of_skills_skill_grade_grade_id",
                schema: "performances",
                table: "group_of_skills");

            migrationBuilder.DropIndex(
                name: "ix_group_of_skills_grade_id",
                schema: "performances",
                table: "group_of_skills");

            migrationBuilder.DropColumn(
                name: "grade_id",
                schema: "performances",
                table: "group_of_skills");
        }
    }
}
