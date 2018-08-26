using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CapStore.Migrations
{
    public partial class CapOrder_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderCaps_Caps_CapId",
                table: "OrderCaps");

            migrationBuilder.AlterColumn<int>(
                name: "CapId",
                table: "OrderCaps",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCaps_Caps_CapId",
                table: "OrderCaps",
                column: "CapId",
                principalTable: "Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderCaps_Caps_CapId",
                table: "OrderCaps");

            migrationBuilder.AlterColumn<int>(
                name: "CapId",
                table: "OrderCaps",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCaps_Caps_CapId",
                table: "OrderCaps",
                column: "CapId",
                principalTable: "Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
