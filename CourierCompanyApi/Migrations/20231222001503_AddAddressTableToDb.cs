using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCompanyApi.Migrations
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    streetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    houseNumber = table.Column<int>(type: "int", nullable: false),
                    flatNumber = table.Column<int>(type: "int", nullable: false),
                    postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
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
