using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class loc_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 40.34857m, 0.384756m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 38.8765m, 0.18798m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 3,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 35.1034m, 2.4678m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 4,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 41.6545m, -1.564m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 5,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 38.999m, -5.938m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 6,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 39.7145m, -4.123m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 7,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 41.0301m, -2.9088m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 0.384756m, 40.34857m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 0.18798m, 38.8765m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 3,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 2.4678m, 35.1034m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 4,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -1.564m, 41.6545m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 5,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -5.938m, 38.999m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 6,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -4.123m, 39.7145m });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 7,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -2.9088m, 41.0301m });
        }
    }
}
