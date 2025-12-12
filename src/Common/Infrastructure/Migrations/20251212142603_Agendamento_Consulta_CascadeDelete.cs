using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Agendamento_Consulta_CascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Agendamentos_AgendamentoId",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_AgendamentoId",
                table: "Consultas");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_ConsultaId",
                table: "Agendamentos",
                column: "ConsultaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Consultas_ConsultaId",
                table: "Agendamentos",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Consultas_ConsultaId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_ConsultaId",
                table: "Agendamentos");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_AgendamentoId",
                table: "Consultas",
                column: "AgendamentoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Agendamentos_AgendamentoId",
                table: "Consultas",
                column: "AgendamentoId",
                principalTable: "Agendamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
