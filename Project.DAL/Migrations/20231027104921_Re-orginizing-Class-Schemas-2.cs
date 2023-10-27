using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReorginizingClassSchemas2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Kullanici_Yemek_ID",
                table: "Yemek_Resimleri",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "Kullanici_Kategori_Detayi",
                columns: table => new
                {
                    ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriMevcudiyetDurum = table.Column<short>(name: "Kategori Mevcudiyet Durum", type: "smallint", nullable: false),
                    KategoriAçıklama = table.Column<string>(name: "Kategori Açıklama", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    KategoriResim = table.Column<string>(name: "Kategori Resim", type: "nvarchar(max)", nullable: true),
                    Kullanici_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kategori_ID = table.Column<short>(type: "smallint", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici_Kategori_Detayi", x => x.ID);
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
                    ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YemekFiyat = table.Column<decimal>(name: "Yemek Fiyat", type: "smallmoney", nullable: false),
                    YemekMevcudiyetDurum = table.Column<short>(name: "Yemek Mevcudiyet Durum", type: "smallint", nullable: false),
                    YemekAçıklama = table.Column<string>(name: "Yemek Açıklama", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    YemekResim = table.Column<string>(name: "Yemek Resim", type: "nvarchar(max)", nullable: true),
                    Kullanici_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Yemek_ID = table.Column<short>(type: "smallint", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici_Yemek_Detayi", x => x.ID);
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

            migrationBuilder.CreateTable(
                name: "Menuler",
                columns: table => new
                {
                    ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuAd = table.Column<string>(name: "Menu Ad", type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MenuDurum = table.Column<short>(name: "Menu Durum", type: "smallint", nullable: false),
                    User_AccessibleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menuler", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menuler_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kullanici_Menu_Detayi",
                columns: table => new
                {
                    ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriAd = table.Column<string>(name: "Kategori Ad", type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MenuID = table.Column<short>(type: "smallint", nullable: false),
                    UserFoodJunctionID = table.Column<short>(type: "smallint", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici_Menu_Detayi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Kullanici_Menu_Detayi_Kullanici_Yemek_Detayi_UserFoodJunctionID",
                        column: x => x.UserFoodJunctionID,
                        principalTable: "Kullanici_Yemek_Detayi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kullanici_Menu_Detayi_Menuler_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menuler",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Yemek_Resimleri_Kullanici_Yemek_ID",
                table: "Yemek_Resimleri",
                column: "Kullanici_Yemek_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Kategori_Detayi_AppUserId",
                table: "Kullanici_Kategori_Detayi",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Kategori_Detayi_Kategori_ID",
                table: "Kullanici_Kategori_Detayi",
                column: "Kategori_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Menu_Detayi_MenuID",
                table: "Kullanici_Menu_Detayi",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Menu_Detayi_UserFoodJunctionID",
                table: "Kullanici_Menu_Detayi",
                column: "UserFoodJunctionID");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Yemek_Detayi_AppUserId",
                table: "Kullanici_Yemek_Detayi",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Yemek_Detayi_Yemek_ID",
                table: "Kullanici_Yemek_Detayi",
                column: "Yemek_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_AppUserId",
                table: "Menuler",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Yemek_Resimleri_Kullanici_Yemek_Detayi_Kullanici_Yemek_ID",
                table: "Yemek_Resimleri",
                column: "Kullanici_Yemek_ID",
                principalTable: "Kullanici_Yemek_Detayi",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yemek_Resimleri_Kullanici_Yemek_Detayi_Kullanici_Yemek_ID",
                table: "Yemek_Resimleri");

            migrationBuilder.DropTable(
                name: "Kullanici_Kategori_Detayi");

            migrationBuilder.DropTable(
                name: "Kullanici_Menu_Detayi");

            migrationBuilder.DropTable(
                name: "Kullanici_Yemek_Detayi");

            migrationBuilder.DropTable(
                name: "Menuler");

            migrationBuilder.DropIndex(
                name: "IX_Yemek_Resimleri_Kullanici_Yemek_ID",
                table: "Yemek_Resimleri");

            migrationBuilder.DropColumn(
                name: "Kullanici_Yemek_ID",
                table: "Yemek_Resimleri");
        }
    }
}
