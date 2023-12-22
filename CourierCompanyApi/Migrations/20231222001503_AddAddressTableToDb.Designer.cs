﻿// <auto-generated />
using CourierCompanyApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CourierCompanyApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231222001503_AddAddressTableToDb")]
    partial class AddAddressTableToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CourierCompanyApi.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("flatNumber")
                        .HasColumnType("int");

                    b.Property<int>("houseNumber")
                        .HasColumnType("int");

                    b.Property<string>("postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("streetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("CourierCompanyApi.Models.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("dimensionsUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("dimensionsUnit")
                        .HasAnnotation("DisplayName", "Unit of package dimensions");

                    b.Property<double>("height")
                        .HasColumnType("float")
                        .HasColumnName("height")
                        .HasAnnotation("DisplayName", "Package height");

                    b.Property<double>("length")
                        .HasColumnType("float")
                        .HasColumnName("length")
                        .HasAnnotation("DisplayName", "Package length");

                    b.Property<double>("weight")
                        .HasColumnType("float")
                        .HasColumnName("weight")
                        .HasAnnotation("DisplayName", "Package weight");

                    b.Property<string>("weightUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("weightUnit")
                        .HasAnnotation("DisplayName", "Unit of package weight");

                    b.Property<double>("width")
                        .HasColumnType("float")
                        .HasColumnName("width")
                        .HasAnnotation("DisplayName", "Package width");

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });
#pragma warning restore 612, 618
        }
    }
}
