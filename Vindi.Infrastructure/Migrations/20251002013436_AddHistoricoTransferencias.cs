using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vindi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoricoTransferencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricoTransferencias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContaOrigemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContaDestinoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoTransferencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoTransferencias_ContasBancarias_ContaDestinoId",
                        column: x => x.ContaDestinoId,
                        principalTable: "ContasBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricoTransferencias_ContasBancarias_ContaOrigemId",
                        column: x => x.ContaOrigemId,
                        principalTable: "ContasBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoTransferencias_ContaDestinoId",
                table: "HistoricoTransferencias",
                column: "ContaDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoTransferencias_ContaOrigemId",
                table: "HistoricoTransferencias",
                column: "ContaOrigemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoTransferencias");
        }
    }
}
