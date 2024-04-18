using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleBTEC.Migrations
{
    /// <inheritdoc />
    public partial class teach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeachName",
                table: "Teaches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeachName",
                table: "Teaches");
        }
    }
}
