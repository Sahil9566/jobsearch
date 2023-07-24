using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class addEducationdetailtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Education_Details",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HighestQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectYourField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    University_Insitute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfGraduation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education_Details", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Education_Details_AspNetUsers_RegisterID",
                        column: x => x.RegisterID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Education_Details_RegisterID",
                table: "Education_Details",
                column: "RegisterID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Education_Details");
        }
    }
}
