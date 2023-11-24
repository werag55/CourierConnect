using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressTable_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "stringName",
                table: "Addresses",
                newName: "streetName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "streetName",
                table: "Addresses",
                newName: "stringName");
        }
    }
}
