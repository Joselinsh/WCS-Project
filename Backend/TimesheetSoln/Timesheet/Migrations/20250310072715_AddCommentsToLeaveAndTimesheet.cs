using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timesheet.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsToLeaveAndTimesheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Timesheets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Leaves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 10, 7, 27, 15, 261, DateTimeKind.Utc).AddTicks(2532), new byte[] { 21, 142, 254, 27, 232, 68, 207, 248, 28, 206, 13, 232, 72, 152, 82, 112, 34, 70, 37, 33, 118, 179, 138, 233, 85, 83, 115, 119, 135, 88, 153, 105, 208, 254, 29, 124, 61, 99, 226, 175, 6, 240, 114, 34, 87, 12, 152, 111, 206, 141, 207, 84, 136, 79, 0, 27, 237, 106, 36, 217, 116, 52, 178, 94 }, new byte[] { 48, 163, 220, 88, 249, 13, 103, 73, 23, 207, 39, 93, 61, 85, 65, 35, 96, 43, 134, 232, 105, 118, 22, 15, 236, 75, 192, 45, 157, 39, 165, 88, 37, 200, 234, 114, 130, 102, 166, 131, 106, 187, 4, 89, 186, 185, 11, 129, 253, 187, 181, 83, 66, 94, 214, 33, 107, 228, 232, 5, 119, 164, 106, 40, 118, 239, 194, 253, 178, 144, 149, 199, 159, 9, 23, 131, 22, 102, 246, 71, 167, 187, 210, 145, 149, 248, 250, 102, 87, 168, 211, 204, 18, 13, 158, 219, 46, 24, 105, 224, 183, 76, 196, 145, 131, 61, 207, 1, 193, 74, 77, 121, 253, 179, 45, 143, 187, 4, 12, 255, 127, 51, 78, 162, 129, 237, 153, 25 }, new DateTime(2025, 3, 10, 7, 27, 15, 261, DateTimeKind.Utc).AddTicks(2533) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Leaves");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 24, 7, 50, 39, 265, DateTimeKind.Utc).AddTicks(5581), new byte[] { 25, 123, 53, 56, 97, 163, 56, 150, 169, 142, 238, 2, 41, 245, 57, 35, 9, 46, 191, 180, 87, 71, 112, 252, 209, 137, 158, 179, 140, 7, 145, 90, 49, 111, 226, 21, 148, 171, 115, 134, 161, 51, 62, 14, 98, 15, 149, 193, 103, 59, 79, 11, 39, 226, 169, 29, 211, 203, 39, 154, 224, 241, 218, 110 }, new byte[] { 195, 50, 149, 181, 20, 57, 237, 244, 130, 88, 242, 252, 170, 197, 100, 222, 59, 120, 70, 211, 224, 61, 155, 108, 64, 139, 130, 0, 13, 248, 75, 186, 165, 148, 205, 216, 144, 136, 195, 254, 205, 41, 226, 121, 13, 44, 228, 125, 150, 53, 93, 216, 143, 241, 28, 229, 110, 24, 116, 108, 216, 85, 93, 252, 211, 192, 226, 139, 130, 177, 177, 20, 24, 99, 134, 114, 20, 112, 229, 1, 102, 222, 112, 87, 147, 74, 170, 41, 249, 5, 160, 96, 211, 182, 246, 13, 70, 234, 54, 176, 116, 108, 210, 241, 75, 162, 105, 61, 62, 86, 136, 64, 120, 248, 81, 251, 63, 238, 18, 45, 181, 234, 230, 218, 60, 230, 215, 232 }, new DateTime(2025, 2, 24, 7, 50, 39, 265, DateTimeKind.Utc).AddTicks(5582) });
        }
    }
}
