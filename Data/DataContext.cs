using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using eClaim.Models;

namespace eClaim.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("ECMEntities")
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<uvwExpenseMaster> uvwExpenseMasters { get; set; }
        public DbSet<uvwExpenseDetail> uvwExpenseDetails { get; set; }
        public DbSet<ExpenseMaster> ExpenseMasters { get; set; }
        public DbSet<ExpenseDetail> ExpenseDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}