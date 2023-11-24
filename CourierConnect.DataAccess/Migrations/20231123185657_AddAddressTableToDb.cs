using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stringName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    houseNumber = table.Column<int>(type: "int", nullable: false),
                    flatNumber = table.Column<int>(type: "int", nullable: false),
                    postcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
