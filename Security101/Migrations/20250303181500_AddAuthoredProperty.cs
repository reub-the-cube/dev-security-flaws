using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Security101.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthoredProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthoredBy",
                table: "ToDoItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthoredBy",
                table: "ToDoItems");
        }
    }
}
