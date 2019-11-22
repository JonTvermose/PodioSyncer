using Microsoft.EntityFrameworkCore.Migrations;

namespace PodioSyncer.Data.Migrations
{
    public partial class AddedRequiredValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "CategoryMappings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Required",
                table: "CategoryMappings");
        }
    }
}
