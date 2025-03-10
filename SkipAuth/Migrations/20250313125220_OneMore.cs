using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkipAuth.Migrations
{
    /// <inheritdoc />
    public partial class OneMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "tokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "tokens_pkey",
                table: "tokens",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "tokens_pkey",
                table: "tokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "tokens");
        }
    }
}
