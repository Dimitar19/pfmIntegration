using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pfm.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Code);
                    table.ForeignKey(
                        name: "FK_categories_categories_ParentCode",
                        column: x => x.ParentCode,
                        principalTable: "categories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BeneficiaryName = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Direction = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Mcc = table.Column<int>(type: "integer", nullable: true),
                    Kind = table.Column<string>(type: "text", nullable: false),
                    CatCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_categories_CatCode",
                        column: x => x.CatCode,
                        principalTable: "categories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_ParentCode",
                table: "categories",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_CatCode",
                table: "transactions",
                column: "CatCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
