using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dbfilter.Migrations
{
    /// <inheritdoc />
    public partial class FillWithLittleTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { "10002014", "Irgend so ein Ding.", "Dingsbums" });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("de4452fc-c898-44fc-bdf9-0401c9c1966b"), "Someplace" });

            migrationBuilder.InsertData(
                table: "StorageBins",
                columns: new[] { "Id", "WarehouseId" },
                values: new object[] { "00-01-00-05-03", new Guid("de4452fc-c898-44fc-bdf9-0401c9c1966b") });

            migrationBuilder.InsertData(
                table: "ProductsAtStorageBins",
                columns: new[] { "ProductId", "StorageBinId", "Quantity" },
                values: new object[] { "10002014", "00-01-00-05-03", 0u });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductsAtStorageBins",
                keyColumns: new[] { "ProductId", "StorageBinId" },
                keyValues: new object[] { "10002014", "00-01-00-05-03" });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "10002014");

            migrationBuilder.DeleteData(
                table: "StorageBins",
                keyColumn: "Id",
                keyValue: "00-01-00-05-03");

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("de4452fc-c898-44fc-bdf9-0401c9c1966b"));
        }
    }
}
