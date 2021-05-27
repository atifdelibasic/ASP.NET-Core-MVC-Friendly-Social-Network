using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class bellnotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "BellNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorId = table.Column<int>(type: "int", nullable: true),
                    NotifierId = table.Column<int>(type: "int", nullable: false),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BellNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BellNotification_AspNetUsers_ActorId",
                        column: x => x.ActorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BellNotification_AspNetUsers_NotifierId",
                        column: x => x.NotifierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BellNotification_NotificationType_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BellNotification_ActorId",
                table: "BellNotification",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_BellNotification_NotificationTypeId",
                table: "BellNotification",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BellNotification_NotifierId",
                table: "BellNotification",
                column: "NotifierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BellNotification");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Notification");
        }
    }
}
