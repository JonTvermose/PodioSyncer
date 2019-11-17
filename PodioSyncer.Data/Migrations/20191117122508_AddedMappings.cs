using Microsoft.EntityFrameworkCore.Migrations;

namespace PodioSyncer.Data.Migrations
{
    public partial class AddedMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AzureFieldName = table.Column<string>(nullable: true),
                    PodioFieldName = table.Column<string>(nullable: true),
                    FieldType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PodioAzureItemLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PodioId = table.Column<int>(nullable: false),
                    PodioRevision = table.Column<int>(nullable: false),
                    AzureId = table.Column<int>(nullable: false),
                    AzureRevision = table.Column<int>(nullable: false),
                    PodioAppId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodioAzureItemLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PodioAzureItemLinks_PodioApps_PodioAppId",
                        column: x => x.PodioAppId,
                        principalTable: "PodioApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PodioValue = table.Column<string>(nullable: true),
                    AzureValue = table.Column<string>(nullable: true),
                    FieldType = table.Column<int>(nullable: false),
                    FieldMappingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryMappings_FieldMappings_FieldMappingId",
                        column: x => x.FieldMappingId,
                        principalTable: "FieldMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMappings_FieldMappingId",
                table: "CategoryMappings",
                column: "FieldMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_PodioAzureItemLinks_PodioAppId",
                table: "PodioAzureItemLinks",
                column: "PodioAppId");

            migrationBuilder.CreateIndex(
                name: "IX_PodioAzureItemLinks_AzureId_PodioId",
                table: "PodioAzureItemLinks",
                columns: new[] { "AzureId", "PodioId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryMappings");

            migrationBuilder.DropTable(
                name: "PodioAzureItemLinks");

            migrationBuilder.DropTable(
                name: "FieldMappings");
        }
    }
}
