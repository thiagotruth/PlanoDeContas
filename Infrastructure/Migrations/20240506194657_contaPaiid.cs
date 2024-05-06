using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class contaPaiid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_Contas_IdContaPai",
                table: "Contas");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_Contas_IdContaPai",
                table: "Contas",
                column: "IdContaPai",
                principalTable: "Contas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_Contas_IdContaPai",
                table: "Contas");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_Contas_IdContaPai",
                table: "Contas",
                column: "IdContaPai",
                principalTable: "Contas",
                principalColumn: "Id");
        }
    }
}
