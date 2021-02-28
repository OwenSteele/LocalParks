using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostcodeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes",
                columns: table => new
                {
                    Zone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Neighbourhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Population = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes", x => x.Zone);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Longitude = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parks", x => x.ParkId);
                    table.ForeignKey(
                        name: "FK_Parks_Postcodes_PostcodeZone",
                        column: x => x.PostcodeZone,
                        principalTable: "Postcodes",
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
                    OrganiserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                table: "Postcodes",
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
                    { 7, new DateTime(2021, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), 41.0301m, -2.9088m, "Shadow Grounds", new DateTime(2021, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), "LP2", 26500 },
                    { 1, new DateTime(2021, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), 40.34857m, 0.384756m, "Leafy Meadows", new DateTime(2021, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified), "LP4", 200000 },
                    { 4, new DateTime(2021, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), 41.6545m, -1.564m, "Shadow Grounds", new DateTime(2021, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), "LP4", 26500 },
                    { 3, new DateTime(2021, 1, 1, 23, 0, 0, 0, DateTimeKind.Unspecified), 35.1034m, 2.4678m, "Mirror Lake Park", new DateTime(2021, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "PF10", 2000 },
                    { 5, new DateTime(2021, 1, 1, 18, 30, 0, 0, DateTimeKind.Unspecified), 38.999m, -5.938m, "Sapphire Gardens", new DateTime(2021, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "PF13", 6000 },
                    { 6, new DateTime(2021, 1, 1, 20, 30, 0, 0, DateTimeKind.Unspecified), 39.7145m, -4.123m, "Fletcher Plaza", new DateTime(2021, 1, 1, 5, 0, 0, 0, DateTimeKind.Unspecified), "PF13", 70000 },
                    { 2, new DateTime(2021, 1, 1, 22, 0, 0, 0, DateTimeKind.Unspecified), 38.8765m, 0.18798m, "Middlesbrook Park", new DateTime(2021, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified), "PF15", 85000 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Date", "Description", "Name", "OrganiserEmail", "OrganiserFirstName", "OrganiserLastName", "OrganiserPhoneNumber", "ParkId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "A fair for everyone, for hobbyists and professionals to come together and meet.", "Arts and Crafts Fair", "JoanRoberts@owensteele.co.uk", "Joan", "Roberts", "07123456789", 1, null },
                    { 2, new DateTime(2021, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Come and race other from the village, egg and spoons provided!", "Egg and Spoon Race", "DanJackson@owensteele.co.uk", "Dan", "Jackson", "0700345876", 2, null },
                    { 3, new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Come and adopt a new feline or canine friend, all looking for good homes.", "Pet adoption Clinic", "MarkDavis@owensteele.co.uk", "Mark", "Davis", "01196596691", 2, null }
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
                    { 7, "0789871231", "Abraham", "Yoluni", "Ramping Cottage, Little Lane", 7, 22850m, new DateTime(2008, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, "07654321000", "Joe", "Bloggs", "123 Chestnut Drive", 1, 35000m, new DateTime(1980, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "0762985702", "Yohan", "Blake", "1 Park Lane", 4, 29000m, new DateTime(2019, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "0784368711", "Robert", "Bob", "Bramble side, Supervisor Street", 3, 35500m, new DateTime(1986, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "0723096876", "Michael", "Mitchell", "154 Wednesday drive", 5, 34000m, new DateTime(2016, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "0787876876", "Daniel", "Cotting", "Little Cottage, Ramping Lane", 6, 26350m, new DateTime(2011, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "07685940321", "Anne", "Smith", "Parkside Cottage, Park Avenue", 2, 32000m, new DateTime(1960, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ParkId",
                table: "Events",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "SportsClubs");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Parks");

            migrationBuilder.DropTable(
                name: "Postcodes");
        }
    }
}
