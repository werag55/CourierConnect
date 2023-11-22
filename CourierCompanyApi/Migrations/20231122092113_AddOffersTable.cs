using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCompanyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddOffersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    creationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "creationDate", "price" },
                values: new object[] { 1, new DateTime(2023, 11, 22, 10, 21, 13, 447, DateTimeKind.Local).AddTicks(5758), 100f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offers");
        }
    }
}
