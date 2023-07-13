using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryPropertyfromMenuDetailEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Yemek Fiyati",
                table: "Menu Detayi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Yemek Fiyati",
                table: "Menu Detayi",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
