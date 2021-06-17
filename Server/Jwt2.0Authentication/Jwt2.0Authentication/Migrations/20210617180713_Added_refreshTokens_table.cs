using Microsoft.EntityFrameworkCore.Migrations;

namespace Jwt2._0Authentication.Migrations
{
    public partial class Added_refreshTokens_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$k6lTGBFMXqMs3P3kd0Bw6..I.7gcb1VBuAcugu68ODf35kOo4os1i");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$MI2ZgcIGUraqFGjWSCThvu.LMNvaLXbNU3Ko1EkdPUftHXIH6snOm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$R7hJennV2BQsllkIS0Flber8LXf6RtHJjATUPFHEw2oRvWUATJmqa");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$8xSqmzBuQU3RcGwmoFcc.OBWKc1bJ98ob0oXI1w4TpEW2FcMxYT9e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$lZavkYYBxELJ4q2MpEH4BOMDVnKuBqP/XrYEHXgv5fJbkFiwK2tHW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$gcXptLkNebHviE47/Pd49.njr8cYmXWvGLu981CLcutE372luxGbG");
        }
    }
}
