using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModelNamesUpdated3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu Detay_Menus_MenuID",
                table: "Menu Detay");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu Detay_Yemekler_FoodID",
                table: "Menu Detay");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_AppUserId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Yemek Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yemek Resimleri",
                table: "Yemek Resimleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menus",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menu Detay",
                table: "Menu Detay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons");

            migrationBuilder.RenameTable(
                name: "Yemek Resimleri",
                newName: "Yemek_Resimleri");

            migrationBuilder.RenameTable(
                name: "Menus",
                newName: "Menuler");

            migrationBuilder.RenameTable(
                name: "Menu Detay",
                newName: "Kullanici_Menu_Detayi");

            migrationBuilder.RenameTable(
                name: "Coupons",
                newName: "Kuponlar");

            migrationBuilder.RenameIndex(
                name: "IX_Yemek Resimleri_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek_Resimleri",
                newName: "IX_Yemek_Resimleri_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID");

            migrationBuilder.RenameIndex(
                name: "IX_Menus_AppUserId",
                table: "Menuler",
                newName: "IX_Menuler_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Menu Detay_FoodID",
                table: "Kullanici_Menu_Detayi",
                newName: "IX_Kullanici_Menu_Detayi_FoodID");

            migrationBuilder.RenameColumn(
                name: "CouponName",
                table: "Kuponlar",
                newName: "KuponID");

            migrationBuilder.RenameColumn(
                name: "CouponExpireDay",
                table: "Kuponlar",
                newName: "Son Kullanım Tarihi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yemek_Resimleri",
                table: "Yemek_Resimleri",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menuler",
                table: "Menuler",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kullanici_Menu_Detayi",
                table: "Kullanici_Menu_Detayi",
                columns: new[] { "MenuID", "FoodID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kuponlar",
                table: "Kuponlar",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanici_Menu_Detayi_Menuler_MenuID",
                table: "Kullanici_Menu_Detayi",
                column: "MenuID",
                principalTable: "Menuler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanici_Menu_Detayi_Yemekler_FoodID",
                table: "Kullanici_Menu_Detayi",
                column: "FoodID",
                principalTable: "Yemekler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Menuler_AspNetUsers_AppUserId",
                table: "Menuler",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yemek_Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek_Resimleri",
                columns: new[] { "UserFoodJunctionAccessibleID", "UserFoodJunctionFoodID" },
                principalTable: "Kullanici_Yemek_Detayi",
                principalColumns: new[] { "Kullanici_ID", "Yemek_ID" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanici_Menu_Detayi_Menuler_MenuID",
                table: "Kullanici_Menu_Detayi");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanici_Menu_Detayi_Yemekler_FoodID",
                table: "Kullanici_Menu_Detayi");

            migrationBuilder.DropForeignKey(
                name: "FK_Menuler_AspNetUsers_AppUserId",
                table: "Menuler");

            migrationBuilder.DropForeignKey(
                name: "FK_Yemek_Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek_Resimleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yemek_Resimleri",
                table: "Yemek_Resimleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menuler",
                table: "Menuler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kuponlar",
                table: "Kuponlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kullanici_Menu_Detayi",
                table: "Kullanici_Menu_Detayi");

            migrationBuilder.RenameTable(
                name: "Yemek_Resimleri",
                newName: "Yemek Resimleri");

            migrationBuilder.RenameTable(
                name: "Menuler",
                newName: "Menus");

            migrationBuilder.RenameTable(
                name: "Kuponlar",
                newName: "Coupons");

            migrationBuilder.RenameTable(
                name: "Kullanici_Menu_Detayi",
                newName: "Menu Detay");

            migrationBuilder.RenameIndex(
                name: "IX_Yemek_Resimleri_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri",
                newName: "IX_Yemek Resimleri_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID");

            migrationBuilder.RenameIndex(
                name: "IX_Menuler_AppUserId",
                table: "Menus",
                newName: "IX_Menus_AppUserId");

            migrationBuilder.RenameColumn(
                name: "Son Kullanım Tarihi",
                table: "Coupons",
                newName: "CouponExpireDay");

            migrationBuilder.RenameColumn(
                name: "KuponID",
                table: "Coupons",
                newName: "CouponName");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanici_Menu_Detayi_FoodID",
                table: "Menu Detay",
                newName: "IX_Menu Detay_FoodID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yemek Resimleri",
                table: "Yemek Resimleri",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menus",
                table: "Menus",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menu Detay",
                table: "Menu Detay",
                columns: new[] { "MenuID", "FoodID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Menu Detay_Menus_MenuID",
                table: "Menu Detay",
                column: "MenuID",
                principalTable: "Menus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Menu Detay_Yemekler_FoodID",
                table: "Menu Detay",
                column: "FoodID",
                principalTable: "Yemekler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_AppUserId",
                table: "Menus",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yemek Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri",
                columns: new[] { "UserFoodJunctionAccessibleID", "UserFoodJunctionFoodID" },
                principalTable: "Kullanici_Yemek_Detayi",
                principalColumns: new[] { "Kullanici_ID", "Yemek_ID" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
