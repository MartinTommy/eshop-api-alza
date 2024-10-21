using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace EshopApiAlza.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImgUri", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "First product description", "https://example.com/img1.jpg", "Product 1", 29.99m },
                    { 2, "Second product description", "https://example.com/img2.jpg", "Product 2", 49.99m },
                    { 3, "Third product description", "https://example.com/img3.jpg", "Product 3", 69.99m },
                    { 4, "Fourth product description", "https://example.com/img4.jpg", "Product 4", 89.99m },
                    { 5, "Fifth product description", "https://example.com/img5.jpg", "Product 5", 109.99m },
                    { 6, "Sixth product description", "https://example.com/img6.jpg", "Product 6", 129.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
