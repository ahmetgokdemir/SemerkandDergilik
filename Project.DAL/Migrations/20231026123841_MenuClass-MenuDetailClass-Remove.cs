using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MenuClassMenuDetailClassRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kullanici_Menu_Detayi");

            migrationBuilder.DropTable(
                name: "Menuler");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menuler",
                columns: table => new
                {
                    ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessibleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    MenuAd = table.Column<string>(name: "Menu Ad", type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MenuDurum = table.Column<short>(name: "Menu Durum", type: "smallint", nullable: false),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true)
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
                    MenuID = table.Column<short>(type: "smallint", nullable: false),
                    FoodID = table.Column<short>(type: "smallint", nullable: false),
                    KategoriAd = table.Column<string>(name: "Kategori Ad", type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici_Menu_Detayi", x => new { x.MenuID, x.FoodID });
                    table.ForeignKey(
                        name: "FK_Kullanici_Menu_Detayi_Menuler_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menuler",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kullanici_Menu_Detayi_Yemekler_FoodID",
                        column: x => x.FoodID,
                        principalTable: "Yemekler",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_Menu_Detayi_FoodID",
                table: "Kullanici_Menu_Detayi",
                column: "FoodID");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_AppUserId",
                table: "Menuler",
                column: "AppUserId");
        }
    }
}
