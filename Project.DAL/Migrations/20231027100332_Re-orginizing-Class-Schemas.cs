using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReorginizingClassSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yemek_Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek_Resimleri");

            migrationBuilder.DropTable(
                name: "Kullanici_Kategori_Detayi");

            migrationBuilder.DropTable(
                name: "Kullanici_Yemek_Detayi");

            migrationBuilder.DropIndex(
                name: "IX_Yemek_Resimleri_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek_Resimleri");

            migrationBuilder.DropColumn(
                name: "UserFoodJunctionAccessibleID",
                table: "Yemek_Resimleri");

            migrationBuilder.DropColumn(
                name: "UserFoodJunctionFoodID",
                table: "Yemek_Resimleri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserFoodJunctionAccessibleID",
                table: "Yemek_Resimleri",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<short>(
                name: "UserFoodJunctionFoodID",
                table: "Yemek_Resimleri",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "Kullanici_Kategori_Detayi",
                columns: table => new
                {
                    Kullanici_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kategori_ID = table.Column<short>(type: "smallint", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KategoriAçıklama = table.Column<string>(name: "Kategori Açıklama", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    KategoriResim = table.Column<string>(name: "Kategori Resim", type: "nvarchar(max)", nullable: true),
                    KategoriMevcudiyetDurum = table.Column<short>(name: "Kategori Mevcudiyet Durum", type: "smallint", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici_Kategori_Detayi", x => new { x.Kullanici_ID, x.Kategori_ID });
                    table.ForeignKey(
                        name: "FK_Kullanici_Kategori_Detayi_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kullanici_Kategori_Detayi_Kategoriler_Kategori_ID",
                        column: x => x.Kategori_ID,
                        principalTable: "Kategoriler",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kullanici_Yemek_Detayi",
                columns: table => new
                {
                    Kullanici_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Yemek_ID = table.Column<short>(type: "smallint", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    YemekAçıklama = table.Column<string>(name: "Yemek Açıklama", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    YemekResim = table.Column<string>(name: "Yemek Resim", type: "nvarchar(max)", nullable: true),
                    YemekFiyat = table.Column<decimal>(name: "Yemek Fiyat", type: "smallmoney", nullable: false),
                    YemekMevcudiyetDurum = table.Column<short>(name: "Yemek Mevcudiyet Durum", type: "smallint", nullable: false),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici_Yemek_Detayi", x => new { x.Kullanici_ID, x.Yemek_ID });
                    table.ForeignKey(
                        name: "FK_Kullanici_Yemek_Detayi_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kullanici_Yemek_Detayi_Yemekler_Yemek_ID",
                        column: x => x.Yemek_ID,
                        principalTable: "Yemekler",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Yemek_Resimleri_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek_Resimleri",
                columns: new[] { "UserFoodJunctionAccessibleID", "UserFoodJunctionFoodID" });

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Kategori_Detayi_AppUserId",
                table: "Kullanici_Kategori_Detayi",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Kategori_Detayi_Kategori_ID",
                table: "Kullanici_Kategori_Detayi",
                column: "Kategori_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Yemek_Detayi_AppUserId",
                table: "Kullanici_Yemek_Detayi",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Yemek_Detayi_Yemek_ID",
                table: "Kullanici_Yemek_Detayi",
                column: "Yemek_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Yemek_Resimleri_Kullanici_Yemek_Detayi_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek_Resimleri",
                columns: new[] { "UserFoodJunctionAccessibleID", "UserFoodJunctionFoodID" },
                principalTable: "Kullanici_Yemek_Detayi",
                principalColumns: new[] { "Kullanici_ID", "Yemek_ID" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
