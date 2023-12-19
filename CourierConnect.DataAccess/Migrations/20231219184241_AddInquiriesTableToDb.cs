using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInquiriesTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pickupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isPriority = table.Column<bool>(type: "bit", nullable: false),
                    weekendDelivery = table.Column<bool>(type: "bit", nullable: false),
                    isCompany = table.Column<bool>(type: "bit", nullable: false),
                    sourceAddressId = table.Column<int>(type: "int", nullable: false),
                    destinationAddressId = table.Column<int>(type: "int", nullable: false),
                    packageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inquiries_Addresses_destinationAddressId",
                        column: x => x.destinationAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inquiries_Addresses_sourceAddressId",
                        column: x => x.sourceAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inquiries_Packages_packageId",
                        column: x => x.packageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_destinationAddressId",
                table: "Inquiries",
                column: "destinationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_packageId",
                table: "Inquiries",
                column: "packageId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_sourceAddressId",
                table: "Inquiries",
                column: "sourceAddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inquiries");
        }
    }
}
