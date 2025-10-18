using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClipNchic.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    descript = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Model",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Ship",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ship", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    discount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: true),
                    start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BlindBox",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collectId = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    descript = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlindBox", x => x.id);
                    table.ForeignKey(
                        name: "FK_BlindBox_Collection_collectId",
                        column: x => x.collectId,
                        principalTable: "Collection",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Base",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    color = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    modelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base", x => x.id);
                    table.ForeignKey(
                        name: "FK_Base_Model_modelId",
                        column: x => x.modelId,
                        principalTable: "Model",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Charm",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    modelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charm", x => x.id);
                    table.ForeignKey(
                        name: "FK_Charm_Model_modelId",
                        column: x => x.modelId,
                        principalTable: "Model",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    totalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    shipPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    payPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    payMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    baseId = table.Column<int>(type: "int", nullable: true),
                    charmId = table.Column<int>(type: "int", nullable: true),
                    productId = table.Column<int>(type: "int", nullable: true),
                    blindBoxId = table.Column<int>(type: "int", nullable: true)

                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collectId = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    descript = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    baseId = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: true),
                    modelId = table.Column<int>(type: "int", nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id);
                    table.ForeignKey(
                        name: "FK_Product_Base_baseId",
                        column: x => x.baseId,
                        principalTable: "Base",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Product_Collection_collectId",
                        column: x => x.collectId,
                        principalTable: "Collection",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Product_Model_modelId",
                        column: x => x.modelId,
                        principalTable: "Model",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Product_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CharmProduct",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: true),
                    charmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharmProduct", x => x.id);
                    table.ForeignKey(
                        name: "FK_CharmProduct_Charm_charmId",
                        column: x => x.charmId,
                        principalTable: "Charm",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CharmProduct_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: true),
                    productId = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Base_modelId",
                table: "Base",
                column: "modelId");

            migrationBuilder.CreateIndex(
                name: "IX_BlindBox_collectId",
                table: "BlindBox",
                column: "collectId");

            migrationBuilder.CreateIndex(
                name: "IX_Charm_modelId",
                table: "Charm",
                column: "modelId");

            migrationBuilder.CreateIndex(
                name: "IX_CharmProduct_charmId",
                table: "CharmProduct",
                column: "charmId");

            migrationBuilder.CreateIndex(
                name: "IX_CharmProduct_productId",
                table: "CharmProduct",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_baseId",
                table: "Image",
                column: "baseId",
                unique: true,
                filter: "[baseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Order_userId",
                table: "Order",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_orderId",
                table: "OrderDetail",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_productId",
                table: "OrderDetail",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_baseId",
                table: "Product",
                column: "baseId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_collectId",
                table: "Product",
                column: "collectId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_modelId",
                table: "Product",
                column: "modelId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_userId",
                table: "Product",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlindBox");

            migrationBuilder.DropTable(
                name: "CharmProduct");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Ship");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Charm");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Base");

            migrationBuilder.DropTable(
                name: "Collection");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Model");
        }
    }
}
