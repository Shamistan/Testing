using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class _FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancialInstitution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FXSettlementDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReconciliationFileID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReconciliationCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettlementCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionAmountcredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReconciliationAmntCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FeeAmountCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionAmountDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReconciliationAmntDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FeeAmountDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CountTotal = table.Column<int>(type: "int", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTransactions_TransactionId",
                table: "SubTransactions",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTransactions");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
