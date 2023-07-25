using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_CategoryFoodJunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Category_of_Foods_Category_of_FoodID",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_Category_of_FoodID",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Category_of_FoodID",
                table: "Foods");

            migrationBuilder.CreateTable(
                name: "Category and Foods Details",
                columns: table => new
                {
                    FoodID = table.Column<int>(type: "int", nullable: false),
                    Category_of_FoodID = table.Column<int>(type: "int", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category and Foods Details", x => new { x.FoodID, x.Category_of_FoodID });
                    table.ForeignKey(
                        name: "FK_Category and Foods Details_Category_of_Foods_Category_of_FoodID",
                        column: x => x.Category_of_FoodID,
                        principalTable: "Category_of_Foods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Category and Foods Details_Foods_FoodID",
                        column: x => x.FoodID,
                        principalTable: "Foods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category and Foods Details_Category_of_FoodID",
                table: "Category and Foods Details",
                column: "Category_of_FoodID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category and Foods Details");

            migrationBuilder.AddColumn<int>(
                name: "Category_of_FoodID",
                table: "Foods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Category_of_FoodID",
                table: "Foods",
                column: "Category_of_FoodID");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Category_of_Foods_Category_of_FoodID",
                table: "Foods",
                column: "Category_of_FoodID",
                principalTable: "Category_of_Foods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
