using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    FreeSpace = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientsGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    BoredIndex = table.Column<int>(type: "int", nullable: false),
                    MaxBoaredIndex = table.Column<int>(type: "int", nullable: false),
                    LunchTimeInSeconds = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientsGroups_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "FreeSpace", "Size" },
                values: new object[,]
                {
                    { new Guid("9aab8ef0-9c99-4a18-a98f-c231dfb01cc8"), 6, 6 },
                    { new Guid("ba6c3686-4e12-4302-92aa-64d07a07c27a"), 5, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientsGroups_TableId",
                table: "ClientsGroups",
                column: "TableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientsGroups");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
