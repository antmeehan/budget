using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AntMeehan.Budget.WebApi
{
    public class BudgetContext : DbContext
    {
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./blog.db");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string GoogleIdentityId { get; set; }
        public string FullName { get; set; }
        public List<Budget> Budgets { get; set; }
    }

    public class Budget
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<RecurringEntry> RecurringEntries { get; set; }
    }

    public class RecurringEntry
    {
        public int Id { get; set; }
        public Recurrance Recurrance { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class ManualEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

    }
    public enum Recurrance
    {
        Weekly = 1,
        Fortnightly = 2,
        Monthly = 3,
        Quartly = 4,
    }
}