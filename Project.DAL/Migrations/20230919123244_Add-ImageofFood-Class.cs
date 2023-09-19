using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddImageofFoodClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Yemek Resimleri",
                columns: table => new
                {
                    ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YemekResim = table.Column<string>(name: "Yemek Resim", type: "nvarchar(max)", nullable: false),
                    IsProfile = table.Column<bool>(type: "bit", nullable: false),
                    UserFoodJunctionAccessibleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserFoodJunctionFoodID = table.Column<short>(type: "smallint", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    CrudDurum = table.Column<short>(name: "Crud Durum", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yemek Resimleri", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Yemek Resimleri_Restoran Yemek Detay_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                        columns: x => new { x.UserFoodJunctionAccessibleID, x.UserFoodJunctionFoodID },
                        principalTable: "Restoran Yemek Detay",
                        principalColumns: new[] { "AccessibleID", "FoodID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Yemek Resimleri_UserFoodJunctionAccessibleID_UserFoodJunctionFoodID",
                table: "Yemek Resimleri",
                columns: new[] { "UserFoodJunctionAccessibleID", "UserFoodJunctionFoodID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Yemek Resimleri");
        }
    }
}
