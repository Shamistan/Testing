using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbContextModels
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }
        public TestDbContext()
        {

        }
        #region DbSets

        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<SubTransactionEntity> SubTransactions { get; set; }

        #endregion DbSets

        #region OnModelCreating Seeding and FluentAPI

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Fulent API

            modelBuilder.Entity<TransactionEntity>(entity =>
            {
                entity.ToTable("Transactions");

                //entity.HasOne(e => e.SubTransactions)
                //    .WithMany(e => e.)
                //    .HasForeignKey(e => e.CategoryId)
                //    .IsRequired()
                //    .OnDelete(DeleteBehavior.Restrict);


            });
            modelBuilder.Entity<SubTransactionEntity>(entity =>
            {
                entity.ToTable("SubTransactions");

                entity.HasOne(e => e.Transaction)
                    .WithMany(e => e.SubTransactions)
                    .HasForeignKey(e => e.TransactionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);


            });


            #endregion Fulent API

            #region Seed
            // modelBuilder.Seed();

            #endregion
        }

        #endregion OnModelCreating Seeding and FluentAPI
    }
}
