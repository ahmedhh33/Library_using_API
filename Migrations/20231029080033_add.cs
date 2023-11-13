using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_web.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookManagements",
                columns: table => new
                {
                    B_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publication_year = table.Column<int>(type: "int", nullable: false),
                    is_Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookManagements", x => x.B_ID);
                });

            migrationBuilder.CreateTable(
                name: "patronManagements",
                columns: table => new
                {
                    Pat_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patronManagements", x => x.Pat_ID);
                });

            migrationBuilder.CreateTable(
                name: "userLogins",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingTransactions",
                columns: table => new
                {
                    TraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    borrowing_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    return_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    B_ID = table.Column<int>(type: "int", nullable: false),
                    Pat_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingTransactions", x => x.TraID);
                    table.ForeignKey(
                        name: "FK_BorrowingTransactions_bookManagements_B_ID",
                        column: x => x.B_ID,
                        principalTable: "bookManagements",
                        principalColumn: "B_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingTransactions_patronManagements_Pat_ID",
                        column: x => x.Pat_ID,
                        principalTable: "patronManagements",
                        principalColumn: "Pat_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingTransactions_B_ID",
                table: "BorrowingTransactions",
                column: "B_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingTransactions_Pat_ID",
                table: "BorrowingTransactions",
                column: "Pat_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowingTransactions");

            migrationBuilder.DropTable(
                name: "userLogins");

            migrationBuilder.DropTable(
                name: "bookManagements");

            migrationBuilder.DropTable(
                name: "patronManagements");
        }
    }
}
