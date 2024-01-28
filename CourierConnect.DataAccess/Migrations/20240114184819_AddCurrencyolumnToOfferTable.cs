using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyolumnToOfferTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currency",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "Offers");
        }
    }
}
