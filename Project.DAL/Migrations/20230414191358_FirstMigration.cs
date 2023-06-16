using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true),
                    RolAdı = table.Column<string>(name: "Rol Adı", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    KullanıcıAdı = table.Column<string>(name: "Kullanıcı Adı", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Category_of_Foods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kategoriİsmi = table.Column<string>(name: "Kategori İsmi", type: "nvarchar(max)", nullable: false),
                    Category_of_FoodPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category_of_Foods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CouponExpireDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ÜrünAdı = table.Column<string>(name: "Ürün Adı", type: "nvarchar(max)", nullable: false),
                    UnitsInStock = table.Column<short>(type: "smallint", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    FoodPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<short>(type: "smallint", nullable: true),
                    Category_of_FoodID = table.Column<int>(type: "int", nullable: false),
                    OluşturulmaTarihi = table.Column<DateTime>(name: "Oluşturulma Tarihi", type: "datetime2", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(name: "Silinme Tarihi", type: "datetime2", nullable: true),
                    GüncellemeTarihi = table.Column<DateTime>(name: "Güncelleme Tarihi", type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Foods_Category_of_Foods_Category_of_FoodID",
                        column: x => x.Category_of_FoodID,
                        principalTable: "Category_of_Foods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Category_of_FoodID",
                table: "Foods",
                column: "Category_of_FoodID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Category_of_Foods");
        }
    }
}
