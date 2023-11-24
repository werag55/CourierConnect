using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class temp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Addresses",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "descAddressID",
                table: "Inquiries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_descAddressID",
                table: "Inquiries",
                column: "descAddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_Addresses_descAddressID",
                table: "Inquiries",
                column: "descAddressID",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inquiries_Addresses_descAddressID",
                table: "Inquiries");

            migrationBuilder.DropIndex(
                name: "IX_Inquiries_descAddressID",
                table: "Inquiries");

            migrationBuilder.DropColumn(
                name: "descAddressID",
                table: "Inquiries");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Addresses",
                newName: "ID");
        }
    }
}
