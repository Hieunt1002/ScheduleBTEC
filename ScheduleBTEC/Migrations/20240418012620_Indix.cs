using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleBTEC.Migrations
{
    /// <inheritdoc />
    public partial class Indix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LearnName",
                table: "Learns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LearnName",
                table: "Learns");
        }
    }
}
