using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTravelApi.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usercompletedschedules_schedules_ScheduleId",
                table: "usercompletedschedules");

            migrationBuilder.DropForeignKey(
                name: "FK_usercompletedschedules_users_UserId",
                table: "usercompletedschedules");

            migrationBuilder.DropIndex(
                name: "IX_usercompletedschedules_ScheduleId",
                table: "usercompletedschedules");

            migrationBuilder.DropIndex(
                name: "IX_usercompletedschedules_UserId",
                table: "usercompletedschedules");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "usercompletedschedules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "usercompletedschedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "usercompletedschedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "usercompletedschedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_usercompletedschedules_ScheduleId",
                table: "usercompletedschedules",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_usercompletedschedules_UserId",
                table: "usercompletedschedules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_usercompletedschedules_schedules_ScheduleId",
                table: "usercompletedschedules",
                column: "ScheduleId",
                principalTable: "schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_usercompletedschedules_users_UserId",
                table: "usercompletedschedules",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
