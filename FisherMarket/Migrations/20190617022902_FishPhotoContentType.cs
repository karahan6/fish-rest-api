using Microsoft.EntityFrameworkCore.Migrations;

namespace FisherMarket.Migrations
{
    public partial class FishPhotoContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Fishes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Fishes");
        }
    }
}
