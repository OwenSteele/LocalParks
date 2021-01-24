using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class LocalParks2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parks",
                columns: new[] { "ParkId", "ClosingTime", "Latitude", "Longitude", "Name", "OpeningTime", "Postcode", "SizeInMetresSquared" },
                values: new object[] { 2, new DateTime(2021, 1, 1, 22, 0, 0, 0, DateTimeKind.Unspecified), 0.18798000000000001, 38.8765, "Middlesbrook Park", new DateTime(2021, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified), "PF15", 85000 });

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 1,
                column: "Members",
                value: 103);

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Date", "Description", "Name", "OrganiserEmail", "OrganiserFirstName", "OrganiserLastName", "OrganiserPhoneNumber", "ParkId" },
                values: new object[,]
                {
                    { 2, new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Come and race other from the village, egg and spoons provided!", "Egg and Spoon Race", "DanJackson@owensteele.co.uk", "Dan", "Jackson", "0700345876", 2 },
                    { 3, new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Come and adopt a new feline or canine friend, all looking for good homes.", "Pet adoption Clinic", "MarkDavis@owensteele.co.uk", "Mark", "Davis", "01196596691", 2 }
                });

            migrationBuilder.InsertData(
                table: "SportsClubs",
                columns: new[] { "ClubId", "Members", "MembershipFee", "Name", "ParkId", "Sport" },
                values: new object[] { 3, 67, 330.0, "Chesterly Cricket Club", 2, 8 });

            migrationBuilder.InsertData(
                table: "Supervisors",
                columns: new[] { "SupervisorId", "FirstName", "LastName", "ParkRef", "Salary", "StartingDate" },
                values: new object[] { 2, "Anne", "Smith", 2, 32000.0, new DateTime(1960, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Supervisors",
                keyColumn: "SupervisorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Parks",
                keyColumn: "ParkId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 1,
                column: "Members",
                value: 100);
        }
    }
}
