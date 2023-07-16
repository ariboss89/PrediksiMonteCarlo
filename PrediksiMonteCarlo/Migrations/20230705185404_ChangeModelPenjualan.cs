using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrediksiMonteCarlo.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModelPenjualan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tanggal",
                table: "Penjualans",
                newName: "Tanggal");

            migrationBuilder.RenameColumn(
                name: "tahun",
                table: "Penjualans",
                newName: "Tahun");

            migrationBuilder.RenameColumn(
                name: "jumlah",
                table: "Penjualans",
                newName: "Jumlah");

            migrationBuilder.RenameColumn(
                name: "bulan",
                table: "Penjualans",
                newName: "Bulan");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Tanggal",
                table: "Penjualans",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tanggal",
                table: "Penjualans",
                newName: "tanggal");

            migrationBuilder.RenameColumn(
                name: "Tahun",
                table: "Penjualans",
                newName: "tahun");

            migrationBuilder.RenameColumn(
                name: "Jumlah",
                table: "Penjualans",
                newName: "jumlah");

            migrationBuilder.RenameColumn(
                name: "Bulan",
                table: "Penjualans",
                newName: "bulan");

            migrationBuilder.AlterColumn<string>(
                name: "tanggal",
                table: "Penjualans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
