using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "accounts",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "accounts",
                newName: "users",
                newSchema: "accounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "accounts",
                table: "users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_users_UserId",
                schema: "accounts",
                table: "user_claims",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_users_UserId",
                schema: "accounts",
                table: "user_logins",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_UserId",
                schema: "accounts",
                table: "user_roles",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_users_UserId",
                schema: "accounts",
                table: "user_tokens",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_users_UserId",
                schema: "accounts",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_users_UserId",
                schema: "accounts",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_UserId",
                schema: "accounts",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_users_UserId",
                schema: "accounts",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "accounts",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "accounts",
                newName: "AspNetUsers",
                newSchema: "accounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "accounts",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_claims",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_logins",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_roles",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_AspNetUsers_UserId",
                schema: "accounts",
                table: "user_tokens",
                column: "UserId",
                principalSchema: "accounts",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
