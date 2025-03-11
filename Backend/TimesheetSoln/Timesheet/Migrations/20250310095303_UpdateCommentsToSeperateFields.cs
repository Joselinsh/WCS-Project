using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timesheet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentsToSeperateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Timesheets",
                newName: "ManagerComments");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Leaves",
                newName: "ManagerComments");

            migrationBuilder.AddColumn<string>(
                name: "HRComments",
                table: "Timesheets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HRComments",
                table: "Leaves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 10, 9, 53, 2, 983, DateTimeKind.Utc).AddTicks(1808), new byte[] { 215, 1, 153, 246, 7, 112, 189, 33, 38, 136, 172, 201, 252, 1, 144, 123, 205, 90, 226, 226, 64, 13, 183, 112, 60, 89, 209, 83, 182, 168, 142, 156, 44, 122, 51, 78, 176, 65, 136, 237, 43, 217, 193, 146, 63, 88, 234, 79, 208, 214, 61, 42, 132, 157, 9, 130, 6, 157, 95, 135, 44, 123, 34, 36 }, new byte[] { 130, 197, 15, 54, 43, 33, 72, 21, 229, 169, 133, 101, 49, 1, 224, 26, 95, 193, 105, 206, 63, 40, 111, 247, 140, 101, 30, 158, 64, 215, 125, 201, 125, 2, 249, 7, 38, 58, 4, 37, 147, 91, 47, 30, 79, 1, 236, 74, 111, 130, 97, 196, 110, 48, 217, 247, 209, 84, 3, 246, 239, 118, 142, 148, 69, 189, 133, 104, 53, 237, 155, 88, 255, 83, 202, 171, 158, 167, 115, 218, 153, 234, 133, 189, 46, 60, 38, 71, 100, 242, 102, 156, 225, 153, 219, 41, 45, 234, 51, 73, 79, 155, 226, 110, 111, 60, 189, 241, 180, 184, 148, 240, 191, 156, 255, 19, 21, 175, 57, 126, 199, 251, 121, 110, 153, 135, 247, 85 }, new DateTime(2025, 3, 10, 9, 53, 2, 983, DateTimeKind.Utc).AddTicks(1809) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HRComments",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "HRComments",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "ManagerComments",
                table: "Timesheets",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "ManagerComments",
                table: "Leaves",
                newName: "Comments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 10, 7, 41, 44, 215, DateTimeKind.Utc).AddTicks(4972), new byte[] { 5, 138, 214, 59, 65, 220, 146, 148, 35, 178, 163, 136, 248, 98, 205, 65, 162, 178, 134, 162, 222, 199, 195, 49, 27, 230, 61, 67, 64, 55, 244, 224, 183, 117, 82, 0, 207, 209, 107, 70, 156, 71, 41, 16, 205, 225, 254, 173, 242, 49, 96, 116, 36, 27, 40, 128, 225, 16, 210, 5, 207, 87, 42, 44 }, new byte[] { 127, 118, 129, 158, 37, 151, 223, 68, 176, 221, 149, 123, 215, 95, 71, 100, 8, 25, 186, 142, 98, 3, 230, 212, 200, 89, 144, 197, 196, 150, 123, 148, 96, 196, 196, 81, 206, 227, 134, 11, 121, 98, 44, 218, 116, 31, 106, 57, 120, 224, 18, 64, 251, 195, 232, 176, 217, 27, 160, 175, 238, 39, 101, 66, 228, 44, 89, 134, 128, 122, 238, 180, 123, 78, 224, 246, 244, 167, 92, 201, 99, 127, 73, 156, 63, 160, 82, 220, 4, 148, 85, 249, 136, 2, 162, 158, 82, 26, 57, 27, 9, 239, 224, 97, 152, 19, 34, 149, 92, 72, 229, 98, 61, 76, 200, 194, 172, 103, 155, 236, 134, 217, 166, 241, 223, 215, 231, 199 }, new DateTime(2025, 3, 10, 7, 41, 44, 215, DateTimeKind.Utc).AddTicks(4973) });
        }
    }
}
