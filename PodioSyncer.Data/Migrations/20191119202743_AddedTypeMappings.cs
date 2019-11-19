using Microsoft.EntityFrameworkCore.Migrations;

namespace PodioSyncer.Data.Migrations
{
    public partial class AddedTypeMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "PodioFieldName",
                table: "FieldMappings",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeMappingId",
                table: "FieldMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TypeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PodioType = table.Column<string>(nullable: true),
                    AzureType = table.Column<string>(nullable: true),
                    PodioAppId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeMappings_PodioApps_PodioAppId",
                        column: x => x.PodioAppId,
                        principalTable: "PodioApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldMappings_TypeMappingId_PodioFieldName",
                table: "FieldMappings",
                columns: new[] { "TypeMappingId", "PodioFieldName" });

            migrationBuilder.CreateIndex(
                name: "IX_TypeMappings_PodioAppId",
                table: "TypeMappings",
                column: "PodioAppId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldMappings_TypeMappings_TypeMappingId",
                table: "FieldMappings",
                column: "TypeMappingId",
                principalTable: "TypeMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldMappings_TypeMappings_TypeMappingId",
                table: "FieldMappings");

            migrationBuilder.DropTable(
                name: "TypeMappings");

            migrationBuilder.DropIndex(
                name: "IX_FieldMappings_TypeMappingId_PodioFieldName",
                table: "FieldMappings");

            migrationBuilder.DropColumn(
                name: "TypeMappingId",
                table: "FieldMappings");

            migrationBuilder.AlterColumn<string>(
                name: "PodioFieldName",
                table: "FieldMappings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PodioAppId",
                table: "FieldMappings",
                type: "int",
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
    }
}
