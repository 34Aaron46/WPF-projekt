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
                    { 1, "Turbófeltöltők és kiegészítők", "Turbó" },
                    { 2, "Dugattyú, hajtókar, csapágy", "Motor belső" },
                    { 3, "Leömlő, katalizátor, hangtompító", "Kipufogó" },
                    { 4, "Légszűrő, szívócső", "Szívórendszer" },
                    { 5, "Csavarmenetes futómű, stabilizátor", "Futómű" },
                    { 6, "Féknyereg, fékbetét, féktárcsa", "Fékrendszer" },
                    { 7, "Motorvezérlő szoftverek", "Chip tuning" }
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
                    { 5, 94000m, 3, 9, "76 mm", "Straight pipe 76 mm" },
                    { 6, 620000m, 1, 3, "+180 LE", "BorgWarner EFR 7163 turbó" },
                    { 7, 135000m, 1, 7, "-25 °C", "Front mount intercooler" },
                    { 8, 52000m, 1, 10, "40 mm", "Wastegate szelep 40 mm" },
                    { 9, 310000m, 2, 3, "1000 LE-ig", "Kovácsolt hajtókar szett" },
                    { 10, 46000m, 2, 8, "13 soros", "Olajhűtő szett" },
                    { 11, 85000m, 3, 6, "200 cellás", "Sportkatalizátor" },
                    { 12, 72000m, 3, 7, "76 mm", "Sport hátsó hangtompító" },
                    { 13, 24000m, 4, 20, "+5 LE", "Sport légszűrő" },
                    { 14, 58000m, 4, 6, "+8 LE", "Szívócső szett" },
                    { 15, 215000m, 5, 5, "30-60 mm süllyesztés", "Csavarmenetes futómű" },
                    { 16, 39000m, 5, 9, "26 mm", "Stabilizátor rúd szett" },
                    { 17, 295000m, 6, 4, "330 mm tárcsa", "Négydugattyús féknyereg szett" },
                    { 18, 32000m, 6, 15, "Racing", "Sport fékbetét" },
                    { 19, 120000m, 7, 30, "+40 LE", "Chip tuning Stage 1" },
                    { 20, 180000m, 7, 20, "+70 LE", "Chip tuning Stage 2" }
                });

            migrationBuilder.InsertData(
                table: "Rendelesek",
                columns: new[] { "Id", "Allapot", "Darab", "TermekId", "VevoNev" },
                values: new object[,]
                {
                    { 1, "Új", 1, 1, "Kovács Bence" },
                    { 2, "Kiszállítva", 2, 13, "Nagy Dávid" },
                    { 3, "Feldolgozás alatt", 1, 15, "Szabó Márk" },
                    { 4, "Kiszállítva", 2, 18, "Tóth Gergő" },
                    { 5, "Új", 1, 19, "Horváth Ádám" },
                    { 6, "Törölve", 1, 6, "Varga Levente" }
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
