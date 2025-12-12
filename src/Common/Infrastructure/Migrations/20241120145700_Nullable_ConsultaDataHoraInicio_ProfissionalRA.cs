using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Nullable_ConsultaDataHoraInicio_ProfissionalRA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            // Tornar Consulta.DataHoraInicio nullable
            migrationBuilder.AlterColumn<DateTime?>(
                name: "DataHoraInicio",
                table: "Consultas",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            // Tornar Profissional.RA nullable
            migrationBuilder.AlterColumn<string>(
                name: "RA",
                table: "Profissionais",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            // Reverter Consulta.DataHoraInicio para não nullable
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataHoraInicio",
                table: "Consultas",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime?),
                oldType: "datetime(6)");

            // Reverter Profissional.RA para não nullable
            migrationBuilder.AlterColumn<string>(
                name: "RA",
                table: "Profissionais",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");
        }
    }
}
