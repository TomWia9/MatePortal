using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class RemoveNumberOfAddToFavColumnFromYerbaMateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAddToFav",
                table: "YerbaMate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfAddToFav",
                table: "YerbaMate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
