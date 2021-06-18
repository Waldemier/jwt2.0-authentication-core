using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jwt2._0Authentication.Migrations
{
    public partial class Added_refreshTokenEntity_ExpirationTimeProp_FIX_FROM_TIMESPAN_TO_DATETIME : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryTime",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$f1YxBqG7ynCbO/uY789vtu65cGXOhI4LHurbPm20eDaL7CjbXCo1q");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$ttJP.KAacKE2GXaXXLw6VOS6vjVOtjeo181tYMFJ5.IPhD9QNCGO6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$PFmLQ8SG0LpGaS4z8yECYe2UKWY1Lijal9wdmgmNQAQvEG.uMTgo2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ExpiryTime",
                table: "RefreshTokens",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
    }
}
