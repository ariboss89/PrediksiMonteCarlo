using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrediksiMonteCarlo.Migrations
{
    /// <inheritdoc />
    public partial class AddTanggalToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bulan",
                table: "Penjualans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tahun",
                table: "Penjualans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tanggal",
                table: "Penjualans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bulan",
                table: "Penjualans");

            migrationBuilder.DropColumn(
                name: "tahun",
                table: "Penjualans");

            migrationBuilder.DropColumn(
                name: "tanggal",
                table: "Penjualans");
        }
    }
}
