using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCompanyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddGUIDColumnsToDeliveryOfferRequestTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAccepted",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "requestStatus",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "requestStatus",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Deliveries");

            migrationBuilder.AddColumn<bool>(
                name: "isAccepted",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
