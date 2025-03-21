﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timesheet.Migrations
{
    /// <inheritdoc />
    public partial class employeerelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 24, 6, 50, 16, 475, DateTimeKind.Utc).AddTicks(2940), new byte[] { 181, 107, 228, 144, 1, 252, 25, 215, 122, 207, 95, 13, 146, 181, 142, 143, 58, 167, 91, 34, 131, 140, 3, 34, 73, 170, 77, 141, 211, 70, 228, 165, 66, 157, 166, 82, 29, 155, 20, 18, 223, 134, 67, 255, 145, 200, 55, 20, 137, 167, 81, 86, 24, 57, 105, 130, 178, 159, 231, 178, 130, 102, 31, 149 }, new byte[] { 18, 86, 149, 255, 25, 191, 176, 119, 205, 61, 159, 149, 122, 22, 139, 156, 173, 186, 91, 64, 194, 195, 226, 151, 242, 47, 148, 43, 245, 136, 79, 60, 209, 222, 162, 57, 163, 153, 93, 131, 205, 183, 121, 253, 13, 35, 229, 197, 105, 123, 51, 83, 59, 83, 122, 254, 170, 148, 52, 163, 54, 11, 22, 93, 231, 24, 112, 211, 206, 4, 114, 67, 107, 188, 242, 179, 239, 52, 57, 124, 115, 188, 132, 243, 252, 194, 141, 64, 199, 106, 71, 204, 176, 125, 164, 251, 40, 126, 101, 139, 235, 222, 131, 81, 113, 245, 8, 80, 16, 64, 86, 12, 79, 38, 147, 187, 45, 225, 53, 214, 190, 254, 21, 170, 27, 249, 214, 215 }, new DateTime(2025, 2, 24, 6, 50, 16, 475, DateTimeKind.Utc).AddTicks(2941) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 10, 20, 50, 3, 85, DateTimeKind.Utc).AddTicks(9442), new byte[] { 135, 188, 224, 62, 22, 217, 192, 236, 112, 130, 86, 206, 20, 10, 85, 33, 212, 54, 79, 255, 175, 231, 190, 54, 226, 123, 236, 180, 63, 193, 117, 239, 212, 50, 41, 226, 159, 200, 21, 161, 111, 175, 48, 23, 36, 195, 95, 6, 71, 141, 166, 213, 177, 135, 47, 127, 6, 55, 24, 39, 15, 252, 108, 100 }, new byte[] { 66, 98, 153, 77, 41, 179, 253, 27, 209, 0, 42, 58, 112, 118, 144, 32, 213, 231, 56, 26, 142, 39, 118, 143, 37, 109, 203, 166, 171, 40, 76, 16, 127, 141, 84, 125, 240, 94, 152, 155, 235, 124, 80, 66, 86, 220, 176, 142, 199, 143, 106, 218, 85, 172, 71, 231, 85, 153, 166, 115, 240, 82, 146, 167, 235, 46, 108, 90, 192, 157, 126, 176, 15, 90, 12, 159, 0, 225, 227, 61, 95, 28, 162, 25, 154, 72, 212, 75, 250, 214, 43, 96, 70, 104, 33, 74, 134, 127, 48, 203, 50, 99, 108, 38, 245, 48, 140, 190, 246, 220, 120, 139, 204, 22, 168, 19, 246, 175, 120, 110, 99, 106, 101, 31, 10, 246, 23, 217 }, new DateTime(2025, 2, 10, 20, 50, 3, 85, DateTimeKind.Utc).AddTicks(9442) });
        }
    }
}
