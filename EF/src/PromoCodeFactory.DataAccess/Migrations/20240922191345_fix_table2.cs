using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fix_table2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_Customers_CustomersId",
                table: "CustomerPreference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPreference_CustomerId",
                table: "CustomerPreference");

            migrationBuilder.DropColumn(
                name: "CustomersId",
                table: "CustomerPreference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference",
                columns: new[] { "CustomerId", "PreferenceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomersId",
                table: "CustomerPreference",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference",
                columns: new[] { "CustomersId", "PreferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPreference_CustomerId",
                table: "CustomerPreference",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_Customers_CustomersId",
                table: "CustomerPreference",
                column: "CustomersId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
