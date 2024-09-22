using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fix_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPreference_CustomersId",
                table: "CustomerPreference");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomerPreference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference",
                columns: new[] { "CustomersId", "PreferenceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CustomerPreference",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPreference_CustomersId",
                table: "CustomerPreference",
                column: "CustomersId");
        }
    }
}
