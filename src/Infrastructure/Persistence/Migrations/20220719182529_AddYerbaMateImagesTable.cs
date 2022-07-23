using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddYerbaMateImagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "YerbaMate");

            migrationBuilder.CreateTable(
                name: "YerbaMateImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    YerbaMateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YerbaMateImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YerbaMateImages_YerbaMate_YerbaMateId",
                        column: x => x.YerbaMateId,
                        principalTable: "YerbaMate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YerbaMateImages_YerbaMateId",
                table: "YerbaMateImages",
                column: "YerbaMateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YerbaMateImages");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "YerbaMate",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
