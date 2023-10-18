using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModelNamesUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restoran Yemek Detay_AspNetUsers_AppUserId",
                table: "Restoran Yemek Detay");

            migrationBuilder.DropForeignKey(
                name: "FK_Restoran Yemek Detay_Yemekler_FoodID",
                table: "Restoran Yemek Detay");

            migrationBuilder.DropForeignKey(
                name: "FK_Yemek Resimleri_Restoran Yemek Detay_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restoran Yemek Detay",
                table: "Restoran Yemek Detay");

            migrationBuilder.RenameTable(
                name: "Restoran Yemek Detay",
                newName: "Kullanici_Yemek_Detayi");

            migrationBuilder.RenameColumn(
                name: "FoodID",
                table: "Kullanici_Yemek_Detayi",
                newName: "Yemek_ID");

            migrationBuilder.RenameColumn(
                name: "AccessibleID",
                table: "Kullanici_Yemek_Detayi",
                newName: "Kullanici_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Restoran Yemek Detay_FoodID",
                table: "Kullanici_Yemek_Detayi",
                newName: "IX_Kullanici_Yemek_Detayi_Yemek_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Restoran Yemek Detay_AppUserId",
                table: "Kullanici_Yemek_Detayi",
                newName: "IX_Kullanici_Yemek_Detayi_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kullanici_Yemek_Detayi",
                table: "Kullanici_Yemek_Detayi",
                columns: new[] { "Kullanici_ID", "Yemek_ID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanici_Yemek_Detayi_AspNetUsers_AppUserId",
                table: "Kullanici_Yemek_Detayi",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanici_Yemek_Detayi_Yemekler_Yemek_ID",
                table: "Kullanici_Yemek_Detayi",
                column: "Yemek_ID",
                principalTable: "Yemekler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yemek Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri",
                columns: new[] { "UserFoodJunctionAccessibleID", "UserFoodJunctionFoodID" },
                principalTable: "Kullanici_Yemek_Detayi",
                principalColumns: new[] { "Kullanici_ID", "Yemek_ID" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanici_Yemek_Detayi_AspNetUsers_AppUserId",
                table: "Kullanici_Yemek_Detayi");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanici_Yemek_Detayi_Yemekler_Yemek_ID",
                table: "Kullanici_Yemek_Detayi");

            migrationBuilder.DropForeignKey(
                name: "FK_Yemek Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kullanici_Yemek_Detayi",
                table: "Kullanici_Yemek_Detayi");

            migrationBuilder.RenameTable(
                name: "Kullanici_Yemek_Detayi",
                newName: "Restoran Yemek Detay");

            migrationBuilder.RenameColumn(
                name: "Yemek_ID",
                table: "Restoran Yemek Detay",
                newName: "FoodID");

            migrationBuilder.RenameColumn(
                name: "Kullanici_ID",
                table: "Restoran Yemek Detay",
                newName: "AccessibleID");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanici_Yemek_Detayi_Yemek_ID",
                table: "Restoran Yemek Detay",
                newName: "IX_Restoran Yemek Detay_FoodID");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanici_Yemek_Detayi_AppUserId",
                table: "Restoran Yemek Detay",
                newName: "IX_Restoran Yemek Detay_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restoran Yemek Detay",
                table: "Restoran Yemek Detay",
                columns: new[] { "AccessibleID", "FoodID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Restoran Yemek Detay_AspNetUsers_AppUserId",
                table: "Restoran Yemek Detay",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restoran Yemek Detay_Yemekler_FoodID",
                table: "Restoran Yemek Detay",
                column: "FoodID",
                principalTable: "Yemekler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yemek Resimleri_Restoran Yemek Detay_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri",
                columns: new[] { "UserFoodJunctionAccessibleID", "UserFoodJunctionFoodID" },
                principalTable: "Restoran Yemek Detay",
                principalColumns: new[] { "AccessibleID", "FoodID" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
