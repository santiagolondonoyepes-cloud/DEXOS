using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DEXOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V20260714_TenantSecurityCrmLoyalty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastPurchaseAt",
                table: "Customers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LifetimeValue",
                table: "Customers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseCount",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CustomerPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PurchasedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPurchases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyCoupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsRedeemed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyCoupons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId_Segment",
                table: "Customers",
                columns: new[] { "TenantId", "Segment" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPurchases_TenantId_CustomerId_PurchasedAt",
                table: "CustomerPurchases",
                columns: new[] { "TenantId", "CustomerId", "PurchasedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyCoupons_TenantId_Code",
                table: "LoyaltyCoupons",
                columns: new[] { "TenantId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPurchases");

            migrationBuilder.DropTable(
                name: "LoyaltyCoupons");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TenantId_Segment",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastPurchaseAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LifetimeValue",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PurchaseCount",
                table: "Customers");
        }
    }
}
