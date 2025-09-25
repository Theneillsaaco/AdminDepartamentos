using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDepartament.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class Addlightcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LightCode",
                table: "UnidadHabitacionals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LightCode",
                table: "UnidadHabitacionals");
        }
    }
}
