using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPulse.Performances.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeskillongroupofskills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_record_skills_skills_skill_id",
                schema: "performances",
                table: "record_skills");

            migrationBuilder.DropIndex(
                name: "ix_record_skills_skill_id",
                schema: "performances",
                table: "record_skills");

            migrationBuilder.DropColumn(
                name: "skill_id",
                schema: "performances",
                table: "record_skills");

            migrationBuilder.AddColumn<Guid>(
                name: "group_id",
                schema: "performances",
                table: "record_skills",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_record_skills_group_id",
                schema: "performances",
                table: "record_skills",
                column: "group_id");

            migrationBuilder.AddForeignKey(
                name: "fk_record_skills_group_of_skills_group_id",
                schema: "performances",
                table: "record_skills",
                column: "group_id",
                principalSchema: "performances",
                principalTable: "group_of_skills",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_record_skills_group_of_skills_group_id",
                schema: "performances",
                table: "record_skills");

            migrationBuilder.DropIndex(
                name: "ix_record_skills_group_id",
                schema: "performances",
                table: "record_skills");

            migrationBuilder.DropColumn(
                name: "group_id",
                schema: "performances",
                table: "record_skills");

            migrationBuilder.AddColumn<Guid>(
                name: "skill_id",
                schema: "performances",
                table: "record_skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_record_skills_skill_id",
                schema: "performances",
                table: "record_skills",
                column: "skill_id");

            migrationBuilder.AddForeignKey(
                name: "fk_record_skills_skills_skill_id",
                schema: "performances",
                table: "record_skills",
                column: "skill_id",
                principalSchema: "performances",
                principalTable: "skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
