using Microsoft.EntityFrameworkCore.Migrations;

namespace PodioSyncer.Data.Migrations
{
    public partial class AddedCategoryPodioValueId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PodioValueId",
                table: "CategoryMappings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PodioValueId",
                table: "CategoryMappings");
        }
    }
}
