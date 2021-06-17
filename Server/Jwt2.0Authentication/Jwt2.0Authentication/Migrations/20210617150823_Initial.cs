using Microsoft.EntityFrameworkCore.Migrations;

namespace Jwt2._0Authentication.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role" },
                values: new object[] { 1, "admin@gmail.com", "Admin", "$2a$11$8xSqmzBuQU3RcGwmoFcc.OBWKc1bJ98ob0oXI1w4TpEW2FcMxYT9e", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role" },
                values: new object[] { 2, "test1@gmail.com", "Dominik", "$2a$11$lZavkYYBxELJ4q2MpEH4BOMDVnKuBqP/XrYEHXgv5fJbkFiwK2tHW", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role" },
                values: new object[] { 3, "test2@gmail.com", "Vladislav", "$2a$11$gcXptLkNebHviE47/Pd49.njr8cYmXWvGLu981CLcutE372luxGbG", 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
