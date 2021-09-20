using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddYerbaMateIdToOpinionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_YerbaMate_YerbaMateId",
                table: "Opinions");

            migrationBuilder.AlterColumn<Guid>(
                name: "YerbaMateId",
                table: "Opinions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_YerbaMate_YerbaMateId",
                table: "Opinions",
                column: "YerbaMateId",
                principalTable: "YerbaMate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_YerbaMate_YerbaMateId",
                table: "Opinions");

            migrationBuilder.AlterColumn<Guid>(
                name: "YerbaMateId",
                table: "Opinions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_YerbaMate_YerbaMateId",
                table: "Opinions",
                column: "YerbaMateId",
                principalTable: "YerbaMate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
