using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCompanyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    offerId = table.Column<int>(type: "int", nullable: false),
                    isAccepted = table.Column<bool>(type: "bit", nullable: false),
                    personalDataId = table.Column<int>(type: "int", nullable: false),
                    rejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Offers_offerId",
                        column: x => x.offerId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_PersonalData_personalDataId",
                        column: x => x.personalDataId,
                        principalTable: "PersonalData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_offerId",
                table: "Requests",
                column: "offerId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_personalDataId",
                table: "Requests",
                column: "personalDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
