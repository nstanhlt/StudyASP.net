using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aow3.Migrations
{
    public partial class addtodofullaudi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                schema: "todo",
                table: "Todos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                schema: "todo",
                table: "Todos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                schema: "todo",
                table: "Todos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                schema: "todo",
                table: "Todos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "todo",
                table: "Todos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                schema: "todo",
                table: "Todos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                schema: "todo",
                table: "Todos",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                schema: "todo",
                table: "Todos");
        }
    }
}
