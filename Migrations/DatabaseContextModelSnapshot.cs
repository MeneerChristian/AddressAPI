﻿// <auto-generated />
using AddressAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AddressAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("AddressAPI.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<string>("HouseNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.Property<string>("Zip")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}