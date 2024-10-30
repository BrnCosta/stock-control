﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockControl.Infrastructure.Context;

#nullable disable

namespace StockControl.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("StockControl.Core.Entities.Stock", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("StockType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Symbol");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockControl.Core.Entities.StockHolder", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("AveragePrice")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StockSymbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StockSymbol")
                        .IsUnique();

                    b.ToTable("StockHolders");
                });

            modelBuilder.Entity("StockControl.Core.Entities.StockOperation", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("OperatingType")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StockSymbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Tax")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("StockSymbol");

                    b.ToTable("StockOperations");
                });

            modelBuilder.Entity("StockControl.Core.Entities.StockHolder", b =>
                {
                    b.HasOne("StockControl.Core.Entities.Stock", "Stock")
                        .WithOne("StockHolder")
                        .HasForeignKey("StockControl.Core.Entities.StockHolder", "StockSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockControl.Core.Entities.StockOperation", b =>
                {
                    b.HasOne("StockControl.Core.Entities.Stock", null)
                        .WithMany("StockOperations")
                        .HasForeignKey("StockSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StockControl.Core.Entities.Stock", b =>
                {
                    b.Navigation("StockHolder");

                    b.Navigation("StockOperations");
                });
#pragma warning restore 612, 618
        }
    }
}
