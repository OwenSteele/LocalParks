using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class LocalParks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parks",
                columns: table => new
                {
                    ParkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeInMetresSquared = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parks", x => x.ParkId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParkId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganiserFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganiserLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganiserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganiserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "ParkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SportsClubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParkId = table.Column<int>(type: "int", nullable: true),
                    Sport = table.Column<int>(type: "int", nullable: false),
                    MembershipFee = table.Column<double>(type: "float", nullable: false),
                    Members = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportsClubs", x => x.ClubId);
                    table.ForeignKey(
                        name: "FK_SportsClubs_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "ParkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    SupervisorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkRef = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.SupervisorId);
                    table.ForeignKey(
                        name: "FK_Supervisors_Parks_ParkRef",
                        column: x => x.ParkRef,
                        principalTable: "Parks",
                        principalColumn: "ParkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parks",
                columns: new[] { "ParkId", "ClosingTime", "Latitude", "Longitude", "Name", "OpeningTime", "Postcode", "SizeInMetresSquared" },
                values: new object[] { 1, new DateTime(2021, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), 0.38475599999999999, 40.348570000000002, "Leafy Meadows", new DateTime(2021, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified), "LP4", 200000 });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Date", "Description", "Name", "OrganiserEmail", "OrganiserFirstName", "OrganiserLastName", "OrganiserPhoneNumber", "ParkId" },
                values: new object[] { 1, new DateTime(2021, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "A fair for everyone, for hobbyists and professionals to come together and meet.", "Arts and Crafts Fair", "JoanRoberts@owensteele.co.uk", "Joan", "Roberts", "07123456789", 1 });

            migrationBuilder.InsertData(
                table: "SportsClubs",
                columns: new[] { "ClubId", "Members", "MembershipFee", "Name", "ParkId", "Sport" },
                values: new object[,]
                {
                    { 1, 100, 20000.0, "Curling Polo Club", 1, 0 },
                    { 2, 20, 150.0, "Meadow RFC", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Supervisors",
                columns: new[] { "SupervisorId", "FirstName", "LastName", "ParkRef", "Salary", "StartingDate" },
                values: new object[] { 1, "Joe", "Bloggs", 1, 35000.0, new DateTime(1980, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ParkId",
                table: "Events",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_SportsClubs_ParkId",
                table: "SportsClubs",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_ParkRef",
                table: "Supervisors",
                column: "ParkRef",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "SportsClubs");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DropTable(
                name: "Parks");
        }
    }
}
