using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddConferenceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpeakerId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conference_users_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_conference",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConferenceId = table.Column<int>(type: "int", nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_conference", x => new { x.UserId, x.ConferenceId });
                    table.ForeignKey(
                        name: "FK_user_conference_conference_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "conference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_conference_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_conference_SpeakerId",
                table: "conference",
                column: "SpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_user_conference_ConferenceId",
                table: "user_conference",
                column: "ConferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_conference");

            migrationBuilder.DropTable(
                name: "conference");
        }
    }
}
