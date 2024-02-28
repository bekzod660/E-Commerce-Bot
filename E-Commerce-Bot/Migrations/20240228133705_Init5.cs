using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Bot.Migrations
{
    /// <inheritdoc />
    public partial class Init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Carts_CartsId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CartsId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Cart",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CartsId",
                table: "Orders");

            migrationBuilder.AlterColumn<double>(
                name: "Longitute",
                table: "Orders",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Orders",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelivered",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelivered",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Orders");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitute",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<int>(
                name: "Cart",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartsId",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartsId",
                table: "Orders",
                column: "CartsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Carts_CartsId",
                table: "Orders",
                column: "CartsId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
