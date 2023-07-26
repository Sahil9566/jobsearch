using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class addJobPrefrencetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jobPrefrences",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KeySkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrefrredRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrefrredLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobPrefrences", x => x.ID);
                    table.ForeignKey(
                        name: "FK_jobPrefrences_AspNetUsers_RegisterID",
                        column: x => x.RegisterID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_jobPrefrences_RegisterID",
                table: "jobPrefrences",
                column: "RegisterID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jobPrefrences");
        }
    }
}
