using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DateNight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DateNightId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DateNights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaTypeId = table.Column<int>(type: "int", nullable: false),
                    PartnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateNights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DateNights_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.AddColumn<int>(
                name: "LikesId",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WatchedById",
                table: "Movie",
                nullable: true);
           
            migrationBuilder.CreateTable(
                name: "UserMovieLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMovieLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMovieLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserMovieLikes_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWatchedMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWatchedMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWatchedMovies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWatchedMovies_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DateNightId",
                table: "Notifications",
                column: "DateNightId");

            migrationBuilder.CreateIndex(
                name: "IX_DateNights_AppUserId",
                table: "DateNights",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieLikes_MovieId",
                table: "UserMovieLikes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieLikes_UserId",
                table: "UserMovieLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchedMovies_MovieId",
                table: "UserWatchedMovies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchedMovies_UserId",
                table: "UserWatchedMovies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_DateNights_DateNightId",
                table: "Notifications",
                column: "DateNightId",
                principalTable: "DateNights",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_DateNights_DateNightId",
                table: "Notifications");


            migrationBuilder.DropTable(
                name: "DateNights");

            migrationBuilder.DropTable(
                name: "UserMovieLikes");

            migrationBuilder.DropTable(
                name: "UserWatchedMovies");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_DateNightId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DateNightId",
                table: "Notifications");
        }
    }
}
