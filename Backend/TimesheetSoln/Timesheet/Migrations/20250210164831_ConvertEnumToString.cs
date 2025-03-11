﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timesheet.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEnumToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 10, 16, 48, 31, 415, DateTimeKind.Utc).AddTicks(9471), new byte[] { 193, 99, 242, 247, 201, 61, 229, 68, 144, 112, 89, 178, 42, 160, 229, 67, 6, 209, 233, 41, 8, 134, 28, 163, 41, 124, 106, 205, 224, 197, 242, 217, 158, 237, 193, 62, 127, 254, 23, 221, 162, 86, 77, 184, 170, 45, 153, 173, 58, 125, 69, 162, 197, 14, 80, 83, 10, 177, 21, 137, 82, 131, 3, 91 }, new byte[] { 217, 54, 121, 205, 66, 96, 135, 236, 240, 148, 125, 221, 59, 237, 214, 27, 88, 175, 130, 218, 154, 114, 137, 142, 165, 48, 254, 20, 81, 51, 184, 85, 172, 91, 66, 232, 119, 5, 64, 142, 131, 171, 67, 227, 99, 123, 11, 85, 229, 193, 1, 4, 92, 142, 122, 207, 238, 5, 181, 207, 232, 31, 101, 225, 152, 71, 191, 130, 49, 34, 81, 224, 70, 242, 82, 53, 47, 225, 165, 57, 59, 127, 7, 69, 222, 254, 97, 54, 215, 210, 191, 165, 38, 213, 198, 195, 183, 251, 238, 206, 85, 114, 83, 84, 122, 94, 235, 240, 186, 4, 219, 164, 176, 240, 92, 13, 4, 67, 129, 64, 121, 76, 42, 136, 135, 19, 195, 112 }, new DateTime(2025, 2, 10, 16, 48, 31, 415, DateTimeKind.Utc).AddTicks(9472) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 10, 16, 47, 42, 532, DateTimeKind.Utc).AddTicks(2071), new byte[] { 159, 1, 150, 106, 92, 194, 176, 111, 3, 50, 128, 13, 72, 169, 72, 88, 123, 238, 39, 207, 37, 228, 124, 178, 193, 202, 215, 238, 116, 207, 230, 66, 237, 142, 174, 184, 112, 103, 139, 110, 52, 223, 43, 191, 139, 179, 96, 165, 232, 15, 119, 239, 233, 104, 225, 17, 50, 22, 27, 205, 185, 234, 62, 152 }, new byte[] { 85, 161, 6, 170, 234, 41, 171, 132, 168, 1, 61, 199, 61, 206, 114, 56, 37, 211, 133, 143, 195, 38, 128, 31, 104, 127, 134, 191, 53, 83, 142, 87, 169, 170, 231, 78, 228, 111, 3, 141, 255, 158, 126, 138, 21, 29, 135, 96, 47, 16, 151, 68, 21, 140, 146, 11, 194, 183, 91, 244, 246, 176, 122, 0, 120, 131, 247, 24, 9, 239, 90, 220, 117, 19, 243, 245, 202, 206, 8, 223, 185, 242, 18, 165, 255, 29, 165, 26, 41, 233, 240, 17, 21, 142, 188, 59, 95, 204, 203, 50, 124, 71, 240, 231, 65, 102, 169, 83, 110, 63, 180, 97, 157, 43, 15, 212, 174, 50, 191, 118, 218, 189, 239, 160, 19, 56, 46, 226 }, new DateTime(2025, 2, 10, 16, 47, 42, 532, DateTimeKind.Utc).AddTicks(2072) });
        }
    }
}
