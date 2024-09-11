using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Bot.Migrations
{
    /// <inheritdoc />
    public partial class UserState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserProcess",
                table: "Users",
                newName: "UserState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserState",
                table: "Users",
                newName: "UserProcess");
        }
    }
}
