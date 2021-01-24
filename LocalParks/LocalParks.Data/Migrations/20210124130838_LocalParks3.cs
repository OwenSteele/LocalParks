using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class LocalParks3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmergencyNumber",
                table: "Supervisors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Office",
                table: "Supervisors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SportsClubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "President",
                table: "SportsClubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "SportsClubs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 1,
                columns: new[] { "Email", "President", "Website" },
                values: new object[] { "CPCPolo@owensteele.co.uk", "Matthrew Albright", "CPCPolo-OS.co.uk" });

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 2,
                columns: new[] { "Email", "President", "Website" },
                values: new object[] { "Contact.Meadow-Rugby@owensteele.co.uk", "Jess Hampston", "Meadow-Rugby-OS.co.uk" });

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 3,
                columns: new[] { "Email", "President", "Website" },
                values: new object[] { "Admin.ChesterlyCricket@owensteele.co.uk", "Linda Paul", "ChesterlyCricketClub-OS.co.uk" });

            migrationBuilder.UpdateData(
                table: "Supervisors",
                keyColumn: "SupervisorId",
                keyValue: 1,
                columns: new[] { "EmergencyNumber", "Office" },
                values: new object[] { "07654321000", "123 Chestnut Drive" });

            migrationBuilder.UpdateData(
                table: "Supervisors",
                keyColumn: "SupervisorId",
                keyValue: 2,
                columns: new[] { "EmergencyNumber", "Office" },
                values: new object[] { "07685940321", "Parkside Cottage, Park Avenue" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmergencyNumber",
                table: "Supervisors");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "Supervisors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "SportsClubs");

            migrationBuilder.DropColumn(
                name: "President",
                table: "SportsClubs");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "SportsClubs");
        }
    }
}
