using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTravelApi.Migrations
{
    /// <inheritdoc />
    public partial class FinalSchemaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Bỏ qua các lệnh DropForeignKey/DropPrimaryKey cũ vì chúng ta sẽ tạo lại bảng mới.
            // Điều này giải quyết lỗi: Can't DROP 'FK_usercompletedschedules_schedules_ScheduleId'

            // =========================================================================================
            // THAO TÁC TRÊN usercompletedschedules: XÓA VÀ TẠO LẠI BẢNG (Giả định đã xóa thủ công ở DB)
            // =========================================================================================

            // Lệnh DropTable này sẽ được chạy nếu bảng đã tồn tại (Chỉ nên dùng nếu không chắc chắn)
            // Nếu bạn đã xóa thủ công, lệnh này an toàn:
            migrationBuilder.Sql("DROP TABLE IF EXISTS `usercompletedschedules`;");
            
            // Xóa bảng tourlocations (giải quyết lỗi Unknown table 'tourlocations' của bạn)
            migrationBuilder.Sql("DROP TABLE IF EXISTS `tourlocations`;");
            
            
            // TẠO LẠI BẢNG usercompletedschedules VỚI CẤU TRÚC MỚI
            migrationBuilder.CreateTable(
                name: "usercompletedschedules",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false), // PK mới
                    countPeople = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true), // Có thể null
                    ScheduleId = table.Column<int>(type: "int", nullable: true) // Có thể null
                    // Thêm các cột khác nếu có
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usercompletedschedules", x => x.BookingId);
                    
                    // Khóa ngoại mới
                    table.ForeignKey(
                        name: "FK_usercompletedschedules_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_usercompletedschedules_schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "schedules",
                        principalColumn: "Id");
                        
                    table.ForeignKey(
                        name: "FK_usercompletedschedules_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_usercompletedschedules_ScheduleId",
                table: "usercompletedschedules",
                column: "ScheduleId");

            // =========================================================================================
            // THAO TÁC TRÊN CÁC BẢNG KHÁC (GIỮ NGUYÊN)
            // =========================================================================================
            
            // Cột Password trên users (chuyển sang nullable)
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            // Cột RefundStatus trên users
            migrationBuilder.AddColumn<bool>(
                name: "RefundStatus",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
                
            // Tạo bảng actualcashs
            migrationBuilder.CreateTable(
                name: "actualcashs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    money = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actualcashs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Tạo bảng banks
            migrationBuilder.CreateTable(
                name: "banks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banks", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Tạo bảng configs
            migrationBuilder.CreateTable(
                name: "configs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    countChangeSchedule = table.Column<int>(type: "int", nullable: false),
                    timeExpiredBookingSec = table.Column<int>(type: "int", nullable: false),
                    timeExpiredOtpSec = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Lệnh Down (Rollback) cần được sửa để khớp với những gì Up() đã làm
            
            migrationBuilder.DropTable(
                name: "usercompletedschedules");

            migrationBuilder.DropTable(
                name: "actualcashs");

            migrationBuilder.DropTable(
                name: "banks");

            migrationBuilder.DropTable(
                name: "configs");

            migrationBuilder.DropColumn(
                name: "RefundStatus",
                table: "users");
            
            // Khôi phục cột Password trên users
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Password",
                keyValue: null,
                column: "Password",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            // Bỏ qua việc khôi phục bảng tourlocations vì nó đã bị xóa vĩnh viễn
            // Bỏ qua việc khôi phục cấu trúc cũ của usercompletedschedules
        }
    }
}