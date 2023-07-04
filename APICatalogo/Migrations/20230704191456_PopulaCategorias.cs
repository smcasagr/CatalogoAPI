using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Para o Postgre é necessário colocar as tabelas e campos entre aspas ¬¬
            migrationBuilder.Sql("INSERT INTO \"Categorias\"(\"Nome\", \"ImagemUrl\") VALUES('Bebidas', 'bebidas.jpg')");
            migrationBuilder.Sql("INSERT INTO \"Categorias\"(\"Nome\", \"ImagemUrl\") VALUES('Lanches', 'lanches.jpg')");
            migrationBuilder.Sql("INSERT INTO \"Categorias\"(\"Nome\", \"ImagemUrl\") VALUES('Sobremesas', 'sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Categorias\"");
        }
    }
}
