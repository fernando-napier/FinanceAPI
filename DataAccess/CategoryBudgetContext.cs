using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceApi.Models;

namespace FinanceApi.DataAccess
{
    public class CategoryBudgetContext : DbContext
    {
        public CategoryBudgetContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<CategoryBudget> CategoryBudget { get; set; }

        public override int SaveChanges()
        {
            addTimestamps();
            return base.SaveChanges();
        }

        private void addTimestamps()
        {

            foreach (var item in this.ChangeTracker.Entries())
            {
                BaseModel bm = item.Entity as BaseModel;

                if (bm != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        bm.DateCreated = DateTime.Now;
                        bm.DateModified = bm.DateCreated;
                    }
                    else if (item.State == EntityState.Modified)
                    {
                        bm.DateModified = DateTime.Now;
                    }
                }
            }
        }
    }
}