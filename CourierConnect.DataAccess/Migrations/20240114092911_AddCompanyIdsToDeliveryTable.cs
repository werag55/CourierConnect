using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIdsToDeliveryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "companyDeliveryId",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "companyId",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "companyDeliveryId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "companyId",
                table: "Deliveries");
        }
    }
}
