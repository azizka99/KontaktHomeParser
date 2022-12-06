﻿// <auto-generated />
using KonatkHomeParser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KonatkHomeParser.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221205234254_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KonatkHomeParser.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(1300)
                        .HasColumnType("nvarchar(1300)");

                    b.Property<string>("LinkToItem")
                        .HasMaxLength(1300)
                        .HasColumnType("nvarchar(1300)");

                    b.Property<string>("Name")
                        .HasMaxLength(1300)
                        .HasColumnType("nvarchar(1300)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });
#pragma warning restore 612, 618
        }
    }
}