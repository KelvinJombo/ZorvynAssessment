using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BackgroundJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialRecords_AspNetUsers_UserId",
                table: "FinancialRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "FinancialRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FinancialRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FinancialRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialRecords_AspNetUsers_UserId",
                table: "FinancialRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialRecords_AspNetUsers_UserId",
                table: "FinancialRecords");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FinancialRecords");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FinancialRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "FinancialRecords",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialRecords_AspNetUsers_UserId",
                table: "FinancialRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
