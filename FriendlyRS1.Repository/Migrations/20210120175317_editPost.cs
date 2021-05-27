using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class editPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HobbyId",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Post_HobbyId",
                table: "Post",
                column: "HobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Hobby_HobbyId",
                table: "Post",
                column: "HobbyId",
                principalTable: "Hobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Hobby_HobbyId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_HobbyId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "HobbyId",
                table: "Post");
        }
    }
}
