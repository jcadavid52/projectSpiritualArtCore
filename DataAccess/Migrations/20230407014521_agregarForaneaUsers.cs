using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class agregarForaneaUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPlan",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdPlan",
                table: "AspNetUsers",
                column: "IdPlan");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Planes_IdPlan",
                table: "AspNetUsers",
                column: "IdPlan",
                principalTable: "Planes",
                principalColumn: "IdPlan",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Planes_IdPlan",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdPlan",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdPlan",
                table: "AspNetUsers");
        }
    }
}
