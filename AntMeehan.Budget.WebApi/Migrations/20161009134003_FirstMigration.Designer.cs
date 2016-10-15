using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AntMeehan.Budget.WebApi;

namespace AntMeehan.Budget.WebApi.Migrations
{
    [DbContext(typeof(BudgetContext))]
    [Migration("20161009134003_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("AntMeehan.Budget.WebApi.Budget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GoogleIdentityId");

                    b.HasKey("Id");

                    b.ToTable("Budgets");
                });
        }
    }
}
