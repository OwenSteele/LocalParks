using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class Refs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Parks_ParkId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_SportsClubs_Parks_ParkId",
                table: "SportsClubs");

            migrationBuilder.DropIndex(
                name: "IX_SportsClubs_ParkId",
                table: "SportsClubs");

            migrationBuilder.DropIndex(
                name: "IX_Events_ParkId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "SportsClubs");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "ParkRef",
                table: "SportsClubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParkRef",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 1,
                column: "ParkRef",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 2,
                column: "ParkRef",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 3,
                column: "ParkRef",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 1,
                column: "ParkRef",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 2,
                column: "ParkRef",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 3,
                column: "ParkRef",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_SportsClubs_ParkRef",
                table: "SportsClubs",
                column: "ParkRef");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ParkRef",
                table: "Events",
                column: "ParkRef");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Parks_ParkRef",
                table: "Events",
                column: "ParkRef",
                principalTable: "Parks",
                principalColumn: "ParkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SportsClubs_Parks_ParkRef",
                table: "SportsClubs",
                column: "ParkRef",
                principalTable: "Parks",
                principalColumn: "ParkId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Parks_ParkRef",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_SportsClubs_Parks_ParkRef",
                table: "SportsClubs");

            migrationBuilder.DropIndex(
                name: "IX_SportsClubs_ParkRef",
                table: "SportsClubs");

            migrationBuilder.DropIndex(
                name: "IX_Events_ParkRef",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ParkRef",
                table: "SportsClubs");

            migrationBuilder.DropColumn(
                name: "ParkRef",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "ParkId",
                table: "SportsClubs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 1,
                column: "ParkId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 2,
                column: "ParkId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 3,
                column: "ParkId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 1,
                column: "ParkId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 2,
                column: "ParkId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SportsClubs",
                keyColumn: "ClubId",
                keyValue: 3,
                column: "ParkId",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_SportsClubs_ParkId",
                table: "SportsClubs",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ParkId",
                table: "Events",
                column: "ParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Parks_ParkId",
                table: "Events",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "ParkId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SportsClubs_Parks_ParkId",
                table: "SportsClubs",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "ParkId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
