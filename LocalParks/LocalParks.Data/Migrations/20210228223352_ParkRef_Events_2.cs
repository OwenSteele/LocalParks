using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalParks.Data.Migrations
{
    public partial class ParkRef_Events_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Parks_ParkId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ParkId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "Events");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Parks_ParkRef",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ParkRef",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ParkRef",
                table: "Events");

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
        }
    }
}
