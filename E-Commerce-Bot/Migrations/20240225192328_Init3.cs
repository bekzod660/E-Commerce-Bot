using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Bot.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Orders_OrderId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_OrderId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Orders",
                newName: "CartsId");

            migrationBuilder.AddColumn<int>(
                name: "Cart",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "CartsId",
                table: "Orders",
                newName: "ItemId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Item",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_OrderId",
                table: "Item",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Orders_OrderId",
                table: "Item",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
