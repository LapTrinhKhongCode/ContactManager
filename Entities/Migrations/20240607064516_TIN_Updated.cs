using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class TIN_Updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TIN",
                table: "Persons",
                newName: "TaxIdentifierNumbers");

            migrationBuilder.AlterColumn<string>(
                name: "TaxIdentifierNumbers",
                table: "Persons",
                type: "nvarchar(8)",
                nullable: true,
                defaultValue: "123ABC",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxIdentifierNumbers",
                table: "Persons",
                newName: "TIN");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldNullable: true,
                oldDefaultValue: "123ABC");
        }
    }
}
