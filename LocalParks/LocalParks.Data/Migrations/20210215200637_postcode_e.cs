using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class postcode_e : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parks_Postcode_PostcodeZone",
                table: "Parks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Postcode",
                table: "Postcode");

            migrationBuilder.RenameTable(
                name: "Postcode",
                newName: "Postcodes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Parks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Parks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Postcodes",
                table: "Postcodes",
                column: "Zone");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Parks_Postcodes_PostcodeZone",
                table: "Parks",
                column: "PostcodeZone",
                principalTable: "Postcodes",
                principalColumn: "Zone",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parks_Postcodes_PostcodeZone",
                table: "Parks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Postcodes",
                table: "Postcodes");

            migrationBuilder.RenameTable(
                name: "Postcodes",
                newName: "Postcode");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Parks",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Parks",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Postcode",
                table: "Postcode",
                column: "Zone");

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 0.38475599999999999, 40.348570000000002 });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 0.18798000000000001, 38.8765 });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 3,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 2.4678, 35.103400000000001 });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 4,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -1.5640000000000001, 41.654499999999999 });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 5,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -5.9379999999999997, 38.999000000000002 });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 6,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -4.1230000000000002, 39.714500000000001 });

            migrationBuilder.UpdateData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 7,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -2.9087999999999998, 41.030099999999997 });

            migrationBuilder.AddForeignKey(
                name: "FK_Parks_Postcode_PostcodeZone",
                table: "Parks",
                column: "PostcodeZone",
                principalTable: "Postcode",
                principalColumn: "Zone",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
