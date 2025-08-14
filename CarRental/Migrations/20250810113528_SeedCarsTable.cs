using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRental.Migrations
{
    /// <inheritdoc />
    public partial class SeedCarsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Brand", "Colour", "Description", "Image", "Model", "PricePerDay", "Status" },
                values: new object[,]
                {
                    { new Guid("1e8f3a5b-7c4d-4e9a-8b1f-9c0a6d5e7f2a"), "Ford", "Orange", "A compact SUV for city and long drives.", "/images/ford_ecosport.jpg", "Ecosport", 2500m, "Available" },
                    { new Guid("94d64bbe-ca9f-4de2-91c6-fc065a646c8f"), "Toyota", "Red", "A comfortable sedan for family trips.", "/images/toyota_camry.jpg", "Camry", 3000m, "Available" },
                    { new Guid("f2c9e8d7-6b5a-4e3d-9a8c-1f0b2e3d4c5b"), "Hyundai", "Red", "A stylish hatchback perfect for the city.", "/images/hyundai_i20.jpg", "i20", 2000m, "Booked" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: new Guid("1e8f3a5b-7c4d-4e9a-8b1f-9c0a6d5e7f2a"));

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: new Guid("94d64bbe-ca9f-4de2-91c6-fc065a646c8f"));

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: new Guid("f2c9e8d7-6b5a-4e3d-9a8c-1f0b2e3d4c5b"));
        }
    }
}
