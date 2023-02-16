using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KWRWebShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class full : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    LoginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.LoginId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_Login_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Login",
                        principalColumn: "LoginId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLine",
                columns: table => new
                {
                    OrderlineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLine", x => x.OrderlineId);
                    table.ForeignKey(
                        name: "FK_OrderLine_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Våben" },
                    { 2, "Våbentilbehør" }
                });

            migrationBuilder.InsertData(
                table: "Login",
                columns: new[] { "LoginId", "Email", "Password", "Type" },
                values: new object[,]
                {
                    { 1, "mail_1@test.dk", "123456789", 0 },
                    { 2, "mail_2@test.dk", "123456789", 1 }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerId", "Address", "Created", "FirstName", "LastName", "LoginId" },
                values: new object[,]
                {
                    { 1, "Langgade 1", new DateTime(2023, 2, 15, 10, 34, 11, 943, DateTimeKind.Local).AddTicks(1297), "Joe", "Mama", 1 },
                    { 2, "Borgmester Christiansens Gade 22", new DateTime(2023, 2, 15, 10, 34, 11, 943, DateTimeKind.Local).AddTicks(1300), "Gabe", "Itch", 2 }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductId", "Brand", "CategoryId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Toyko Marui", 1, "Airsoft pistol md BlowBack.", "Glock 18", 899m },
                    { 2, "CYMA", 2, "Fuld automatisk assault rifle.", "AK47", 1599m }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "OrderId", "Created", "CustomerId", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 2, 15, 10, 34, 11, 943, DateTimeKind.Local).AddTicks(1323), 1, 1898m },
                    { 2, new DateTime(2023, 2, 15, 10, 34, 11, 943, DateTimeKind.Local).AddTicks(1325), 2, 1599m }
                });

            migrationBuilder.InsertData(
                table: "OrderLine",
                columns: new[] { "OrderlineId", "Amount", "OrderId", "Price", "ProductId" },
                values: new object[,]
                {
                    { 1, 1, 1, 899m, 1 },
                    { 2, 1, 1, 999m, 1 },
                    { 3, 1, 2, 1599m, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_LoginId",
                table: "Customer",
                column: "LoginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_OrderId",
                table: "OrderLine",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLine");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Login");
        }
    }
}
