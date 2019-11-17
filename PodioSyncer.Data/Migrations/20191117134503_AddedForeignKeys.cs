using Microsoft.EntityFrameworkCore.Migrations;

namespace PodioSyncer.Data.Migrations
{
    public partial class AddedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PodioAppId",
                table: "FieldMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FieldMappings_PodioAppId",
                table: "FieldMappings",
                column: "PodioAppId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldMappings_PodioApps_PodioAppId",
                table: "FieldMappings",
                column: "PodioAppId",
                principalTable: "PodioApps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldMappings_PodioApps_PodioAppId",
                table: "FieldMappings");

            migrationBuilder.DropIndex(
                name: "IX_FieldMappings_PodioAppId",
                table: "FieldMappings");

            migrationBuilder.DropColumn(
                name: "PodioAppId",
                table: "FieldMappings");
        }
    }
}
