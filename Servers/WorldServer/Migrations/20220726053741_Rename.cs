using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldServer.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FooData",
                table: "Foo",
                newName: "Data");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Foo",
                newName: "FooData");
        }
    }
}
