using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddYerbaMateIdToFavouritesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_YerbaMate_YerbaMateId",
                table: "Favourites");

            migrationBuilder.AlterColumn<Guid>(
                name: "YerbaMateId",
                table: "Favourites",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_YerbaMate_YerbaMateId",
                table: "Favourites",
                column: "YerbaMateId",
                principalTable: "YerbaMate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_YerbaMate_YerbaMateId",
                table: "Favourites");

            migrationBuilder.AlterColumn<Guid>(
                name: "YerbaMateId",
                table: "Favourites",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_YerbaMate_YerbaMateId",
                table: "Favourites",
                column: "YerbaMateId",
                principalTable: "YerbaMate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
