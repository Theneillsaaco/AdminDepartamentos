using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDepartamentos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIndexFromPagosRetrasado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pagos_Retrasado",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Interesados_TipoUnidadHabitacional",
                table: "Interesados");

            migrationBuilder.CreateIndex(
                name: "IX_Interesados_TipoUnidadHabitacional",
                table: "Interesados",
                column: "TipoUnidadHabitacional");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Interesados_TipoUnidadHabitacional",
                table: "Interesados");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Retrasado",
                table: "Pagos",
                column: "Retrasado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interesados_TipoUnidadHabitacional",
                table: "Interesados",
                column: "TipoUnidadHabitacional",
                unique: true);
        }
    }
}
