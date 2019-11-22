using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PodioSyncer.Data.Migrations
{
    public partial class AddedSyncEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AzureUrl",
                table: "PodioAzureItemLinks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PodioUrl",
                table: "PodioAzureItemLinks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SyncEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SyncDate = table.Column<DateTime>(nullable: false),
                    Initiator = table.Column<int>(nullable: false),
                    PodioRevision = table.Column<int>(nullable: false),
                    AzureRevision = table.Column<int>(nullable: false),
                    PodioAzureItemLinkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyncEvents_PodioAzureItemLinks_PodioAzureItemLinkId",
                        column: x => x.PodioAzureItemLinkId,
                        principalTable: "PodioAzureItemLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SyncEvents_PodioAzureItemLinkId",
                table: "SyncEvents",
                column: "PodioAzureItemLinkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncEvents");

            migrationBuilder.DropColumn(
                name: "AzureUrl",
                table: "PodioAzureItemLinks");

            migrationBuilder.DropColumn(
                name: "PodioUrl",
                table: "PodioAzureItemLinks");
        }
    }
}
