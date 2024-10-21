﻿// <auto-generated />
using EshopApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EshopApiAlza.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241021095842_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EshopApi.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "First product description",
                            ImgUri = "https://example.com/img1.jpg",
                            Name = "Product 1",
                            Price = 29.99m
                        },
                        new
                        {
                            Id = 2,
                            Description = "Second product description",
                            ImgUri = "https://example.com/img2.jpg",
                            Name = "Product 2",
                            Price = 49.99m
                        },
                        new
                        {
                            Id = 3,
                            Description = "Third product description",
                            ImgUri = "https://example.com/img3.jpg",
                            Name = "Product 3",
                            Price = 69.99m
                        },
                        new
                        {
                            Id = 4,
                            Description = "Fourth product description",
                            ImgUri = "https://example.com/img4.jpg",
                            Name = "Product 4",
                            Price = 89.99m
                        },
                        new
                        {
                            Id = 5,
                            Description = "Fifth product description",
                            ImgUri = "https://example.com/img5.jpg",
                            Name = "Product 5",
                            Price = 109.99m
                        },
                        new
                        {
                            Id = 6,
                            Description = "Sixth product description",
                            ImgUri = "https://example.com/img6.jpg",
                            Name = "Product 6",
                            Price = 129.99m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
