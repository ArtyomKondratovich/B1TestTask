﻿// <auto-generated />
using System;
using B1TestTask.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace B1TestTask.DataAccess.Migrations
{
    [DbContext(typeof(TaskDbContext))]
    partial class FirstTaskDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("B1TestTask.Domain.Entities.BankAccountClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AccountClasses");
                });

            modelBuilder.Entity("B1TestTask.Domain.Entities.ExelReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("B1TestTask.Domain.Entities.ExelRow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BankAccount")
                        .HasColumnType("integer");

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<double>("ClosedBalanceAsset")
                        .HasColumnType("double precision");

                    b.Property<double>("ClosedBalanceLiability")
                        .HasColumnType("double precision");

                    b.Property<double>("OpeningBalanceAsset")
                        .HasColumnType("double precision");

                    b.Property<double>("OpeningBalanceLiability")
                        .HasColumnType("double precision");

                    b.Property<int>("ReportId")
                        .HasColumnType("integer");

                    b.Property<double>("TurnoverCredit")
                        .HasColumnType("double precision");

                    b.Property<double>("TurnoverDebit")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("ReportId");

                    b.ToTable("ExelRows");
                });

            modelBuilder.Entity("B1TestTask.Domain.Entities.FileLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("DoubleNumber")
                        .HasColumnType("double precision");

                    b.Property<int>("IntNumber")
                        .HasColumnType("integer");

                    b.Property<string>("LatinStr")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MergedFileId")
                        .HasColumnType("integer");

                    b.Property<string>("RussianStr")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MergedFileId");

                    b.ToTable("FileLines");
                });

            modelBuilder.Entity("B1TestTask.Domain.Entities.MergedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("B1TestTask.Domain.Entities.ExelRow", b =>
                {
                    b.HasOne("B1TestTask.Domain.Entities.BankAccountClass", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("B1TestTask.Domain.Entities.ExelReport", "Report")
                        .WithMany()
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Report");
                });

            modelBuilder.Entity("B1TestTask.Domain.Entities.FileLine", b =>
                {
                    b.HasOne("B1TestTask.Domain.Entities.MergedFile", "MergedFile")
                        .WithMany()
                        .HasForeignKey("MergedFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MergedFile");
                });
#pragma warning restore 612, 618
        }
    }
}
