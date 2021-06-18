using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jwt2._0Authentication.Migrations
{
    public partial class Added_refreshTokenEntity_ExpirationTimeProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpiryTime",
                table: "RefreshTokens",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$aRP0Iatsc2ADBEO8Fn4yM.txgtiwsMfJR8C/VfNZj2KcsbYEntSTu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$DYbYw2x0/TU9iZQiCnHTeuvPvg1yXMHbDbeW0mnFjZYP0wkLjFN.6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$CnnEoiLCErWTDSql0WtI.u8Z4hk9mERScVyBK5C8hY72XgT8r3ITu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryTime",
                table: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$WsG5kYxGtS7xtFTrP3IUp.JVp7Dh7Lx6odW0g6OV6DvLLXXiQu7dW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$fjcBFvYnxH4.UwnUqFg8/.R3UzFwFg5Rp8U63RPYLNusLxxB1CDXy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$Y8LulVb5TSEcOmShXm7yNeuoerl9wLZFbD2bui9G21fGCEr/IzwP6");
        }
    }
}
