using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Consulta_Equipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Equipes_EquipeId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_EquipeId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "EquipeId",
                table: "Agendamentos");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipeId",
                table: "Consultas",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_EquipeId",
                table: "Consultas",
                column: "EquipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Equipes_EquipeId",
                table: "Consultas",
                column: "EquipeId",
                principalTable: "Equipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Equipes_EquipeId",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_EquipeId",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "EquipeId",
                table: "Consultas");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipeId",
                table: "Agendamentos",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_EquipeId",
                table: "Agendamentos",
                column: "EquipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Equipes_EquipeId",
                table: "Agendamentos",
                column: "EquipeId",
                principalTable: "Equipes",
                principalColumn: "Id");
        }
    }
}
