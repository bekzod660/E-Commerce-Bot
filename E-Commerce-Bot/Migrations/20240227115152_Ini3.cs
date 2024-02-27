using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Bot.Migrations
{
    /// <inheritdoc />
    public partial class Ini3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessHelper_Users_UserId",
                table: "ProcessHelper");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessHelper",
                table: "ProcessHelper");

            migrationBuilder.RenameTable(
                name: "ProcessHelper",
                newName: "ProcessHelpers");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessHelper_UserId",
                table: "ProcessHelpers",
                newName: "IX_ProcessHelpers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessHelpers",
                table: "ProcessHelpers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessHelpers_Users_UserId",
                table: "ProcessHelpers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessHelpers_Users_UserId",
                table: "ProcessHelpers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessHelpers",
                table: "ProcessHelpers");

            migrationBuilder.RenameTable(
                name: "ProcessHelpers",
                newName: "ProcessHelper");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessHelpers_UserId",
                table: "ProcessHelper",
                newName: "IX_ProcessHelper_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessHelper",
                table: "ProcessHelper",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessHelper_Users_UserId",
                table: "ProcessHelper",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
