using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddBrandIdAndCategoryIdToYerbaMatesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YerbaMate_Brands_BrandId",
                table: "YerbaMate");

            migrationBuilder.DropForeignKey(
                name: "FK_YerbaMate_Categories_CategoryId",
                table: "YerbaMate");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "YerbaMate",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "YerbaMate",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_YerbaMate_Brands_BrandId",
                table: "YerbaMate",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YerbaMate_Categories_CategoryId",
                table: "YerbaMate",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YerbaMate_Brands_BrandId",
                table: "YerbaMate");

            migrationBuilder.DropForeignKey(
                name: "FK_YerbaMate_Categories_CategoryId",
                table: "YerbaMate");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "YerbaMate",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "YerbaMate",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_YerbaMate_Brands_BrandId",
                table: "YerbaMate",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_YerbaMate_Categories_CategoryId",
                table: "YerbaMate",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
