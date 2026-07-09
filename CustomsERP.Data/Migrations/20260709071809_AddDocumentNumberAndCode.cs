using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomsERP.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentNumberAndCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Documents",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Documents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Documents",
                newName: "Type");
        }
    }
}
