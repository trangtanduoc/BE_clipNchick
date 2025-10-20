using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClipNchic.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderDetailBlindBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "blindBoxId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_blindBoxId",
                table: "OrderDetail",
                column: "blindBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_BlindBox_blindBoxId",
                table: "OrderDetail",
                column: "blindBoxId",
                principalTable: "BlindBox",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_BlindBox_blindBoxId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_blindBoxId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "blindBoxId",
                table: "OrderDetail");
        }
    }
}
