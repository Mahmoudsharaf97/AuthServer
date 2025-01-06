using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsYakeenNationalIdVerified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsYakeenNationalIdVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsYakeenNationalIdVerified",
                table: "AspNetUsers");
        }
    }
}
