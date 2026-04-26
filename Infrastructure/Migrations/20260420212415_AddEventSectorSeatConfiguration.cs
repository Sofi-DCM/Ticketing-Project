using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventSectorSeatConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Sectors_SectorId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Sectors_Events_EventId",
                table: "Sectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sectors",
                table: "Sectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Sectors",
                newName: "SECTOR");

            migrationBuilder.RenameTable(
                name: "Seats",
                newName: "SEAT");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "EVENT");

            migrationBuilder.RenameIndex(
                name: "IX_Sectors_EventId",
                table: "SECTOR",
                newName: "IX_SECTOR_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_SectorId",
                table: "SEAT",
                newName: "IX_SEAT_SectorId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "SECTOR",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SECTOR",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SEAT",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RowIdentifier",
                table: "SEAT",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SEAT",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "EVENT",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "EVENT",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EVENT",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SECTOR",
                table: "SECTOR",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SEAT",
                table: "SEAT",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EVENT",
                table: "EVENT",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SEAT_SECTOR_SectorId",
                table: "SEAT",
                column: "SectorId",
                principalTable: "SECTOR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SECTOR_EVENT_EventId",
                table: "SECTOR",
                column: "EventId",
                principalTable: "EVENT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SEAT_SECTOR_SectorId",
                table: "SEAT");

            migrationBuilder.DropForeignKey(
                name: "FK_SECTOR_EVENT_EventId",
                table: "SECTOR");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SECTOR",
                table: "SECTOR");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SEAT",
                table: "SEAT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EVENT",
                table: "EVENT");

            migrationBuilder.RenameTable(
                name: "SECTOR",
                newName: "Sectors");

            migrationBuilder.RenameTable(
                name: "SEAT",
                newName: "Seats");

            migrationBuilder.RenameTable(
                name: "EVENT",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_SECTOR_EventId",
                table: "Sectors",
                newName: "IX_Sectors_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_SEAT_SectorId",
                table: "Seats",
                newName: "IX_Seats_SectorId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Sectors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sectors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "RowIdentifier",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sectors",
                table: "Sectors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Sectors_SectorId",
                table: "Seats",
                column: "SectorId",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sectors_Events_EventId",
                table: "Sectors",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
