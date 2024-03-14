using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IsAccepted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_AuthorId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_ReceieverId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_DateNights_DateNightId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "DateNightId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "DateNights",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_AuthorId",
                table: "Notifications",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_ReceieverId",
                table: "Notifications",
                column: "ReceieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_DateNights_DateNightId",
                table: "Notifications",
                column: "DateNightId",
                principalTable: "DateNights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_AuthorId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_ReceieverId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_DateNights_DateNightId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "DateNights");

            migrationBuilder.AlterColumn<int>(
                name: "DateNightId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_AuthorId",
                table: "Notifications",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_ReceieverId",
                table: "Notifications",
                column: "ReceieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_DateNights_DateNightId",
                table: "Notifications",
                column: "DateNightId",
                principalTable: "DateNights",
                principalColumn: "Id");
        }
    }
}
