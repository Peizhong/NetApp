﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetApp.Service.Models;

namespace NetApp.Service.Migrations
{
    [DbContext(typeof(MallContext))]
    [Migration("20180812041303_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NetApp.Entities.Mall.Category", b =>
                {
                    b.Property<string>("CategoryId");

                    b.Property<string>("CategoryName");

                    b.Property<int>("CategoryType");

                    b.Property<string>("FullPath");

                    b.Property<int>("IsShow");

                    b.Property<string>("ParentCategoryId");

                    b.Property<string>("Remark");

                    b.Property<double?>("SortNo");

                    b.Property<DateTime?>("UpdateTime");

                    b.HasKey("CategoryId");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("ActualPrice");

                    b.Property<DateTime?>("CreateTime");

                    b.Property<int>("Status");

                    b.Property<decimal>("TotalPrice");

                    b.Property<string>("TransactionId");

                    b.Property<DateTime?>("UpdateTime");

                    b.Property<string>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.OrderDetail", b =>
                {
                    b.Property<string>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OrderId");

                    b.Property<string>("ProductId");

                    b.Property<int>("Quantity");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.Product", b =>
                {
                    b.Property<string>("ProductId");

                    b.Property<string>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("ManufacturerId");

                    b.Property<decimal>("Price");

                    b.Property<string>("ProductName");

                    b.Property<string>("Remark");

                    b.Property<DateTime?>("UpdateTime");

                    b.Property<string>("VendorId");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.User", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.Category", b =>
                {
                    b.HasOne("NetApp.Entities.Mall.Category", "ParentCategory")
                        .WithMany("Categories")
                        .HasForeignKey("ParentCategoryId");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.Order", b =>
                {
                    b.HasOne("NetApp.Entities.Mall.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.OrderDetail", b =>
                {
                    b.HasOne("NetApp.Entities.Mall.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId");

                    b.HasOne("NetApp.Entities.Mall.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("NetApp.Entities.Mall.Product", b =>
                {
                    b.HasOne("NetApp.Entities.Mall.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");
                });
#pragma warning restore 612, 618
        }
    }
}
