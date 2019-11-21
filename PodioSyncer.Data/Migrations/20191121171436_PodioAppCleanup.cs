using Microsoft.EntityFrameworkCore.Migrations;

namespace PodioSyncer.Data.Migrations
{
    public partial class PodioAppCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "PodioApps");

            migrationBuilder.AlterColumn<int>(
                name: "PodioAppId",
                table: "PodioApps",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PodioTypeExternalId",
                table: "PodioApps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PodioTypeExternalId",
                table: "PodioApps");

            migrationBuilder.AlterColumn<string>(
                name: "PodioAppId",
                table: "PodioApps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PodioApps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
