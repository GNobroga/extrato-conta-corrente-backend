using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InsertNaTabelaLancamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            """
                INSERT INTO "Lancamentos" ("Descricao", "Data", "Valor", "Avulso", "Status") VALUES 
                    ('Lancamento1', '2023-11-16', 100.50, false, 'Válido'),
                    ('Lancamento2', '2023-11-17', 75.20, true, 'Cancelado'),
                    ('Lancamento3', '2023-11-18', 120.75, false, 'Válido');
            """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Lancamentos");
        }
    }
}
