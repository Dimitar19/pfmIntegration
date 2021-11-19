using Microsoft.EntityFrameworkCore.Migrations;

namespace pfm.Migrations
{
    public partial class TransactionSplitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transactionSplits",
                columns: table => new
                {
                    CatCode = table.Column<string>(type: "text", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactionSplits", x => new { x.CatCode, x.TransactionId });
                    table.ForeignKey(
                        name: "FK_transactionSplits_categories_CatCode",
                        column: x => x.CatCode,
                        principalTable: "categories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactionSplits_transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactionSplits_TransactionId",
                table: "transactionSplits",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactionSplits");
        }
    }
}
