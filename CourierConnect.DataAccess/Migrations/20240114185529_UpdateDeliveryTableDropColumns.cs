using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryTableDropColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancelationDeadline",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "courierName",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "courierSurname",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "deliveryDate",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "deliveryStatus",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "pickUpDate",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "reason",
                table: "Deliveries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "cancelationDeadline",
                table: "Deliveries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "courierName",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "courierSurname",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "deliveryDate",
                table: "Deliveries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "deliveryStatus",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "pickUpDate",
                table: "Deliveries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reason",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
