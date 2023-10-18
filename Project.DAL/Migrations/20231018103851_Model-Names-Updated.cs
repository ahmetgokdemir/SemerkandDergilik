using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModelNamesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restoran Kategori Detay_AspNetUsers_AppUserId",
                table: "Restoran Kategori Detay");

            migrationBuilder.DropForeignKey(
                name: "FK_Restoran Kategori Detay_Kategoriler_CategoryofFoodID",
                table: "Restoran Kategori Detay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restoran Kategori Detay",
                table: "Restoran Kategori Detay");

            migrationBuilder.RenameTable(
                name: "Restoran Kategori Detay",
                newName: "Kullanici_Kategori_Detayi");

            migrationBuilder.RenameColumn(
                name: "CategoryofFoodID",
                table: "Kullanici_Kategori_Detayi",
                newName: "Kategori_ID");

            migrationBuilder.RenameColumn(
                name: "AccessibleID",
                table: "Kullanici_Kategori_Detayi",
                newName: "Kullanici_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Restoran Kategori Detay_CategoryofFoodID",
                table: "Kullanici_Kategori_Detayi",
                newName: "IX_Kullanici_Kategori_Detayi_Kategori_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Restoran Kategori Detay_AppUserId",
                table: "Kullanici_Kategori_Detayi",
                newName: "IX_Kullanici_Kategori_Detayi_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kullanici_Kategori_Detayi",
                table: "Kullanici_Kategori_Detayi",
                columns: new[] { "Kullanici_ID", "Kategori_ID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanici_Kategori_Detayi_AspNetUsers_AppUserId",
                table: "Kullanici_Kategori_Detayi",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanici_Kategori_Detayi_Kategoriler_Kategori_ID",
                table: "Kullanici_Kategori_Detayi",
                column: "Kategori_ID",
                principalTable: "Kategoriler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanici_Kategori_Detayi_AspNetUsers_AppUserId",
                table: "Kullanici_Kategori_Detayi");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanici_Kategori_Detayi_Kategoriler_Kategori_ID",
                table: "Kullanici_Kategori_Detayi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kullanici_Kategori_Detayi",
                table: "Kullanici_Kategori_Detayi");

            migrationBuilder.RenameTable(
                name: "Kullanici_Kategori_Detayi",
                newName: "Restoran Kategori Detay");

            migrationBuilder.RenameColumn(
                name: "Kategori_ID",
                table: "Restoran Kategori Detay",
                newName: "CategoryofFoodID");

            migrationBuilder.RenameColumn(
                name: "Kullanici_ID",
                table: "Restoran Kategori Detay",
                newName: "AccessibleID");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanici_Kategori_Detayi_Kategori_ID",
                table: "Restoran Kategori Detay",
                newName: "IX_Restoran Kategori Detay_CategoryofFoodID");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanici_Kategori_Detayi_AppUserId",
                table: "Restoran Kategori Detay",
                newName: "IX_Restoran Kategori Detay_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restoran Kategori Detay",
                table: "Restoran Kategori Detay",
                columns: new[] { "AccessibleID", "CategoryofFoodID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Restoran Kategori Detay_AspNetUsers_AppUserId",
                table: "Restoran Kategori Detay",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restoran Kategori Detay_Kategoriler_CategoryofFoodID",
                table: "Restoran Kategori Detay",
                column: "CategoryofFoodID",
                principalTable: "Kategoriler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
