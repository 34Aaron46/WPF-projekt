using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TuningKatalogusApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kategoriak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nev = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Leiras = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriak", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Termekek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nev = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MuszakiAdat = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Ar = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Keszlet = table.Column<int>(type: "int", nullable: false),
                    KategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termekek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Termekek_Kategoriak_KategoriaId",
                        column: x => x.KategoriaId,
                        principalTable: "Kategoriak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rendelesek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    VevoNev = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    TermekId = table.Column<int>(type: "int", nullable: false),
                    Darab = table.Column<int>(type: "int", nullable: false),
                    Allapot = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rendelesek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rendelesek_Termekek_TermekId",
                        column: x => x.TermekId,
                        principalTable: "Termekek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Kategoriak",
                columns: new[] { "Id", "Leiras", "Nev" },
                values: new object[,]
                {
                    { 1, "Turbófeltöltők", "Turbó" },
                    { 2, "Dugattyú, hajtókar, csapágy", "Motor belső" },
                    { 3, "Leömlő, straight pipe", "Kipufogó" }
                });

            migrationBuilder.InsertData(
                table: "Termekek",
                columns: new[] { "Id", "Ar", "KategoriaId", "Keszlet", "MuszakiAdat", "Nev" },
                values: new object[,]
                {
                    { 1, 489000m, 1, 5, "+140 LE", "Garrett G30-770 turbó" },
                    { 2, 238000m, 2, 6, "11,5 bar", "Kovácsolt dugattyú szett" },
                    { 3, 68000m, 2, 12, "0,025 mm", "Versenycsapágy szett" },
                    { 4, 179000m, 3, 4, "+12 LE", "Tuning leömlő 4-2-1" },
                    { 5, 94000m, 3, 9, "76 mm", "Straight pipe 76 mm" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rendelesek_TermekId",
                table: "Rendelesek",
                column: "TermekId");

            migrationBuilder.CreateIndex(
                name: "IX_Termekek_KategoriaId",
                table: "Termekek",
                column: "KategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rendelesek");

            migrationBuilder.DropTable(
                name: "Termekek");

            migrationBuilder.DropTable(
                name: "Kategoriak");
        }
    }
}
