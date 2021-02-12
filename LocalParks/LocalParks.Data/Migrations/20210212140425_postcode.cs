using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class postcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postcode",
                columns: table => new
                {
                    Zone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Neighbourhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Population = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcode", x => x.Zone);
                });

            migrationBuilder.CreateTable(
                name: "Parks",
                columns: table => new
                {
                    ParkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostcodeZone = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Parks_Postcode_PostcodeZone",
                        column: x => x.PostcodeZone,
                        principalTable: "Postcode",
                        principalColumn: "Zone",
                        onDelete: ReferentialAction.Restrict);
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
                    MembershipFee = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Members = table.Column<int>(type: "int", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    President = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Office = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                table: "Postcode",
                columns: new[] { "Zone", "Neighbourhood", "Population" },
                values: new object[,]
                {
                    { "LP1", "Ringhurst", 274987L },
                    { "LP2", "MapleDown", 16570L },
                    { "LP3", "Alterdon", 98105L },
                    { "LP4", "Rakeley", 7654L },
                    { "PF10", "Chapterborough", 100311L },
                    { "PF11", "MarketTown", 3948L },
                    { "PF12", "Snowbush", 10334L },
                    { "PF13", "Romsby", 12808L },
                    { "PF14", "Accreton", 6510L },
                    { "PF15", "Caelfall", 968L }
                });

            migrationBuilder.InsertData(
                table: "Parks",
                columns: new[] { "ParkId", "ClosingTime", "Latitude", "Longitude", "Name", "OpeningTime", "PostcodeZone", "SizeInMetresSquared" },
                values: new object[,]
                {
                    { 7, new DateTime(2021, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), -2.9087999999999998, 41.030099999999997, "Shadow Grounds", new DateTime(2021, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), "LP2", 26500 },
                    { 1, new DateTime(2021, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), 0.38475599999999999, 40.348570000000002, "Leafy Meadows", new DateTime(2021, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified), "LP4", 200000 },
                    { 4, new DateTime(2021, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), -1.5640000000000001, 41.654499999999999, "Shadow Grounds", new DateTime(2021, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), "LP4", 26500 },
                    { 3, new DateTime(2021, 1, 1, 23, 0, 0, 0, DateTimeKind.Unspecified), 2.4678, 35.103400000000001, "Mirror Lake Park", new DateTime(2021, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "PF10", 2000 },
                    { 5, new DateTime(2021, 1, 1, 18, 30, 0, 0, DateTimeKind.Unspecified), -5.9379999999999997, 38.999000000000002, "Sapphire Gardens", new DateTime(2021, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "PF13", 6000 },
                    { 6, new DateTime(2021, 1, 1, 20, 30, 0, 0, DateTimeKind.Unspecified), -4.1230000000000002, 39.714500000000001, "Fletcher Plaza", new DateTime(2021, 1, 1, 5, 0, 0, 0, DateTimeKind.Unspecified), "PF13", 70000 },
                    { 2, new DateTime(2021, 1, 1, 22, 0, 0, 0, DateTimeKind.Unspecified), 0.18798000000000001, 38.8765, "Middlesbrook Park", new DateTime(2021, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified), "PF15", 85000 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Date", "Description", "Name", "OrganiserEmail", "OrganiserFirstName", "OrganiserLastName", "OrganiserPhoneNumber", "ParkId" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "A fair for everyone, for hobbyists and professionals to come together and meet.", "Arts and Crafts Fair", "JoanRoberts@owensteele.co.uk", "Joan", "Roberts", "07123456789", 1 },
                    { 2, new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Come and race other from the village, egg and spoons provided!", "Egg and Spoon Race", "DanJackson@owensteele.co.uk", "Dan", "Jackson", "0700345876", 2 },
                    { 3, new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Come and adopt a new feline or canine friend, all looking for good homes.", "Pet adoption Clinic", "MarkDavis@owensteele.co.uk", "Mark", "Davis", "01196596691", 2 }
                });

            migrationBuilder.InsertData(
                table: "SportsClubs",
                columns: new[] { "ClubId", "Email", "Members", "MembershipFee", "Name", "ParkId", "President", "Sport", "Website" },
                values: new object[,]
                {
                    { 1, "CPCPolo@owensteele.co.uk", 103, 20000m, "Curling Polo Club", 1, "Matthrew Albright", 0, "CPCPolo-OS.co.uk" },
                    { 2, "Contact.Meadow-Rugby@owensteele.co.uk", 20, 150m, "Meadow RFC", 1, "Jess Hampston", 2, "Meadow-Rugby-OS.co.uk" },
                    { 3, "Admin.ChesterlyCricket@owensteele.co.uk", 67, 330m, "Chesterly Cricket Club", 2, "Linda Paul", 8, "ChesterlyCricketClub-OS.co.uk" }
                });

            migrationBuilder.InsertData(
                table: "Supervisors",
                columns: new[] { "SupervisorId", "EmergencyNumber", "FirstName", "LastName", "Office", "ParkRef", "Salary", "StartingDate" },
                values: new object[,]
                {
                    { 1, "07654321000", "Joe", "Bloggs", "123 Chestnut Drive", 1, 35000m, new DateTime(1980, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "0762985702", "Yohan", "Blake", "1 Park Lane", 4, 29000m, new DateTime(2019, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "0784368711", "Robert", "Bob", "Bramble side, Supervisor Street", 3, 35500m, new DateTime(1986, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "0723096876", "Michael", "Mitchell", "154 Wednesday drive", 5, 34000m, new DateTime(2016, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "0787876876", "Daniel", "Cotting", "Little Cottage, Ramping Lane", 6, 26350m, new DateTime(2011, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "07685940321", "Anne", "Smith", "Parkside Cottage, Park Avenue", 2, 32000m, new DateTime(1960, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ParkId",
                table: "Events",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Parks_PostcodeZone",
                table: "Parks",
                column: "PostcodeZone");

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

            migrationBuilder.DropTable(
                name: "Postcode");
        }
    }
}
