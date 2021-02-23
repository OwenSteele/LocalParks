using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LocalParks.Data.Migrations
{
    public partial class postcode_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Supervisors",
                keyColumn: "SupervisorId",
                keyValue: 5,
                column: "StartingDate",
                value: new DateTime(2016, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Supervisors",
                columns: new[] { "SupervisorId", "EmergencyNumber", "FirstName", "LastName", "Office", "ParkRef", "Salary", "StartingDate" },
                values: new object[] { 7, "0789871231", "Abraham", "Yoluni", "Ramping Cottage, Little Lane", 7, 22850m, new DateTime(2008, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Supervisors",
                keyColumn: "SupervisorId",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Supervisors",
                keyColumn: "SupervisorId",
                keyValue: 5,
                column: "StartingDate",
                value: new DateTime(2016, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
