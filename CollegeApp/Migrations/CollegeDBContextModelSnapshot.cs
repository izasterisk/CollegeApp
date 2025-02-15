﻿// <auto-generated />
using System;
using CollegeApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CollegeApp.Migrations
{
    [DbContext(typeof(CollegeDBContext))]
    partial class CollegeDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("CollegeApp.Data.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Departments", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DepartmentName = "ECE",
                            Description = "ECE"
                        },
                        new
                        {
                            Id = 2,
                            DepartmentName = "CSE",
                            Description = "CSE"
                        });
                });

            modelBuilder.Entity("CollegeApp.Data.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(450)
                        .HasColumnType("varchar(450)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("studentName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Students", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Hanoi",
                            DOB = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "xnxx@gmail.com",
                            studentName = "YoungWxrdie"
                        },
                        new
                        {
                            Id = 2,
                            Address = "Hanoi",
                            DOB = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "xvideos@gmail.com",
                            studentName = "OldMCK"
                        });
                });

            modelBuilder.Entity("CollegeApp.Data.Student", b =>
                {
                    b.HasOne("CollegeApp.Data.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentId")
                        .HasConstraintName("FK_Students_Department");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("CollegeApp.Data.Department", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
