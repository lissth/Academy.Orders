using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.OrdersTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialTablesT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdersT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemsT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemsT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemsT_OrdersT_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrdersT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusHistoryT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistoryT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistoryT_OrdersT_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrdersT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemsT_OrderId",
                table: "OrderItemsT",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistoryT_OrderId",
                table: "OrderStatusHistoryT",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemsT");

            migrationBuilder.DropTable(
                name: "OrderStatusHistoryT");

            migrationBuilder.DropTable(
                name: "OrdersT");
        }
    }
}
