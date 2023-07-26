using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class addprofessionaltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Professional_Details",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegiserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Experience = table.Column<bool>(type: "bit", nullable: false),
                    CurrentLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professional_Details", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Professional_Details_AspNetUsers_RegiserID",
                        column: x => x.RegiserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professional_Details_RegiserID",
                table: "Professional_Details",
                column: "RegiserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Professional_Details");
        }
    }
}
