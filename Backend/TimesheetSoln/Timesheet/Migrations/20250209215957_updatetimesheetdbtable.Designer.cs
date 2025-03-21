﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Timesheet.Data;

#nullable disable

namespace Timesheet.Migrations
{
    [DbContext(typeof(TimesheetDbContext))]
    [Migration("20250209215957_updatetimesheetdbtable")]
    partial class updatetimesheetdbtable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Timesheet.Models.Admins", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Timesheet.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Timesheet.Models.HR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("HRs");
                });

            modelBuilder.Entity("Timesheet.Models.TimesheetDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("HoursWorked")
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Timesheets");
                });

            modelBuilder.Entity("Timesheet.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateOnly?>("JoiningDate")
                        .HasColumnType("date");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Unassigned");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 2, 9, 21, 59, 57, 19, DateTimeKind.Utc).AddTicks(8448),
                            DateOfBirth = new DateOnly(2002, 6, 6),
                            Department = "Admin",
                            Designation = "Admin",
                            Email = "admin@timesheet.com",
                            FullName = "Admin",
                            JoiningDate = new DateOnly(2025, 1, 1),
                            PasswordHash = new byte[] { 81, 19, 184, 214, 52, 85, 121, 248, 86, 17, 216, 217, 99, 131, 7, 119, 162, 44, 100, 39, 51, 59, 166, 186, 72, 62, 128, 126, 82, 135, 83, 239, 209, 29, 92, 223, 61, 209, 67, 104, 182, 50, 62, 201, 242, 72, 5, 158, 34, 191, 197, 27, 7, 100, 69, 116, 152, 17, 217, 170, 209, 97, 246, 207 },
                            PasswordSalt = new byte[] { 155, 29, 92, 244, 27, 44, 79, 30, 198, 239, 106, 21, 6, 208, 44, 230, 133, 127, 213, 158, 21, 21, 121, 208, 9, 211, 234, 160, 10, 183, 5, 114, 123, 30, 150, 46, 27, 2, 26, 51, 255, 218, 113, 37, 190, 109, 63, 158, 5, 221, 195, 62, 180, 52, 139, 74, 2, 140, 87, 254, 249, 67, 241, 61, 89, 212, 105, 239, 167, 58, 200, 143, 2, 30, 90, 100, 147, 39, 214, 201, 29, 11, 153, 134, 101, 167, 59, 182, 0, 233, 119, 205, 114, 10, 255, 209, 67, 67, 79, 227, 158, 115, 116, 61, 184, 174, 70, 93, 185, 121, 162, 137, 59, 18, 67, 126, 4, 161, 242, 118, 39, 126, 238, 221, 130, 228, 130, 164 },
                            PhoneNumber = "9876543456",
                            Role = "Admin",
                            UpdatedAt = new DateTime(2025, 2, 9, 21, 59, 57, 19, DateTimeKind.Utc).AddTicks(8449)
                        });
                });

            modelBuilder.Entity("Timesheet.Models.Employee", b =>
                {
                    b.HasOne("Timesheet.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Timesheet.Models.HR", b =>
                {
                    b.HasOne("Timesheet.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Timesheet.Models.TimesheetDb", b =>
                {
                    b.HasOne("Timesheet.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });
#pragma warning restore 612, 618
        }
    }
}
