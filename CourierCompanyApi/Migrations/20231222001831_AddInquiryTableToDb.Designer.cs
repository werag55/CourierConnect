﻿// <auto-generated />
using System;
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
    [Migration("20231222001831_AddInquiryTableToDb")]
    partial class AddInquiryTableToDb
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
                        .HasColumnType("int")
                        .HasColumnName("flatNumber")
                        .HasAnnotation("DisplayName", "Flat number");

                    b.Property<int>("houseNumber")
                        .HasColumnType("int")
                        .HasColumnName("houseNumber")
                        .HasAnnotation("DisplayName", "House number");

                    b.Property<string>("postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postCode")
                        .HasAnnotation("DisplayName", "Postcode");

                    b.Property<string>("streetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("streetName")
                        .HasAnnotation("DisplayName", "Street name");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("CourierCompanyApi.Models.Courier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("CourierCompanyApi.Models.Inquiry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("deliveryDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("deliveryDate")
                        .HasAnnotation("DisplayName", "Delivery date");

                    b.Property<int>("destinationAddressId")
                        .HasColumnType("int");

                    b.Property<bool>("isCompany")
                        .HasColumnType("bit")
                        .HasColumnName("isCompany")
                        .HasAnnotation("DisplayName", "Company");

                    b.Property<bool>("isPriority")
                        .HasColumnType("bit")
                        .HasColumnName("isPriority")
                        .HasAnnotation("DisplayName", "Priority");

                    b.Property<int>("packageId")
                        .HasColumnType("int");

                    b.Property<DateTime>("pickupDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("pickupDate")
                        .HasAnnotation("DisplayName", "Pickup date");

                    b.Property<int>("sourceAddressId")
                        .HasColumnType("int");

                    b.Property<bool>("weekendDelivery")
                        .HasColumnType("bit")
                        .HasColumnName("weekendDelivery")
                        .HasAnnotation("DisplayName", "Delivery at weekend");

                    b.HasKey("Id");

                    b.HasIndex("destinationAddressId");

                    b.HasIndex("packageId");

                    b.HasIndex("sourceAddressId");

                    b.ToTable("Inquiries");
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

            modelBuilder.Entity("CourierCompanyApi.Models.PersonalData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("addressId")
                        .HasColumnType("int");

                    b.Property<string>("companyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("addressId");

                    b.ToTable("PersonalData");
                });

            modelBuilder.Entity("CourierCompanyApi.Models.Inquiry", b =>
                {
                    b.HasOne("CourierCompanyApi.Models.Address", "destinationAddress")
                        .WithMany()
                        .HasForeignKey("destinationAddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CourierCompanyApi.Models.Package", "package")
                        .WithMany()
                        .HasForeignKey("packageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CourierCompanyApi.Models.Address", "sourceAddress")
                        .WithMany()
                        .HasForeignKey("sourceAddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("destinationAddress");

                    b.Navigation("package");

                    b.Navigation("sourceAddress");
                });

            modelBuilder.Entity("CourierCompanyApi.Models.PersonalData", b =>
                {
                    b.HasOne("CourierCompanyApi.Models.Address", "address")
                        .WithMany()
                        .HasForeignKey("addressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_PersonalData_Address_addressId");

                    b.Navigation("address");
                });
#pragma warning restore 612, 618
        }
    }
}
