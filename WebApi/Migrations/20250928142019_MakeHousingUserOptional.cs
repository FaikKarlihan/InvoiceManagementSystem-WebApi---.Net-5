using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class MakeHousingUserOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HousingId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "Housings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Housings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_HousingId",
                table: "Users",
                column: "HousingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Housings_HousingId",
                table: "Users",
                column: "HousingId",
                principalTable: "Housings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Housings_HousingId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_HousingId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HousingId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "Housings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Housings");
        }
    }
}
