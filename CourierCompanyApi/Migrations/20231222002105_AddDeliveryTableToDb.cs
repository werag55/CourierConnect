using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCompanyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Offers_offerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_PersonalData_personalDataId",
                table: "Requests");

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courierId = table.Column<int>(type: "int", nullable: false),
                    requestId = table.Column<int>(type: "int", nullable: false),
                    pickUpDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deliveryStatus = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Couriers_courierId",
                        column: x => x.courierId,
                        principalTable: "Couriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Requests_requestId",
                        column: x => x.requestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_courierId",
                table: "Deliveries",
                column: "courierId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_requestId",
                table: "Deliveries",
                column: "requestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Offers_offerId",
                table: "Requests",
                column: "offerId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_PersonalData_personalDataId",
                table: "Requests",
                column: "personalDataId",
                principalTable: "PersonalData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Offers_offerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_PersonalData_personalDataId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Offers_offerId",
                table: "Requests",
                column: "offerId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_PersonalData_personalDataId",
                table: "Requests",
                column: "personalDataId",
                principalTable: "PersonalData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
