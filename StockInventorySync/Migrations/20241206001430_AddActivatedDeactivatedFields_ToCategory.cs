using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockInventorySync.Migrations
{
    /// <inheritdoc />
    public partial class AddActivatedDeactivatedFields_ToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivatedDeactivatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateActivatedDeactivated",
                table: "Categories",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivatedDeactivatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DateActivatedDeactivated",
                table: "Categories");
        }
    }
}
