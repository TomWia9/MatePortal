using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddedRelationBetweenBrandsAndYerbaMate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "YerbaMate",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YerbaMate_BrandId",
                table: "YerbaMate",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_YerbaMate_Brands_BrandId",
                table: "YerbaMate",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YerbaMate_Brands_BrandId",
                table: "YerbaMate");

            migrationBuilder.DropIndex(
                name: "IX_YerbaMate_BrandId",
                table: "YerbaMate");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "YerbaMate");
        }
    }
}
