using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTravelApi.Migrations
{
    /// <inheritdoc />
    public partial class FixedDatabaseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "countChangeSchedule",
                table: "configs");

            migrationBuilder.DropColumn(
                name: "timeExpiredBookingHour",
                table: "configs");

            migrationBuilder.RenameColumn(
                name: "timeExpiredOtpSec",
                table: "configs",
                newName: "Value");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "configs",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "bookings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Qr",
                table: "bookings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "configs");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "Qr",
                table: "bookings");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "configs",
                newName: "timeExpiredOtpSec");

            migrationBuilder.AddColumn<int>(
                name: "countChangeSchedule",
                table: "configs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "timeExpiredBookingHour",
                table: "configs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
