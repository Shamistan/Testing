﻿// <auto-generated />
using Infrastructure.DbContextModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20211205214940__FirstMigration")]
    partial class _FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ApplicationCore.Entities.SubTransactionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CountTotal")
                        .HasColumnType("int");

                    b.Property<decimal>("FeeAmountCredit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FeeAmountDebit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("NetValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ReconciliationAmntCredit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ReconciliationAmntDebit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SettlementCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TransactionAmountDebit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TransactionAmountcredit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TransactionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("SubTransactions");
                });

            modelBuilder.Entity("ApplicationCore.Entities.TransactionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("FXSettlementDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FinancialInstitution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReconciliationCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReconciliationFileID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("ApplicationCore.Entities.SubTransactionEntity", b =>
                {
                    b.HasOne("ApplicationCore.Entities.TransactionEntity", "Transaction")
                        .WithMany("SubTransactions")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("ApplicationCore.Entities.TransactionEntity", b =>
                {
                    b.Navigation("SubTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
