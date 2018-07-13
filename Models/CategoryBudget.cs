using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinanceApi.Models
{

    public enum BudgetInterval { DAILY, WEEKLY, MONTHLY, YEARLY }
    public class CategoryBudget : BaseModel
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public long CategoryID { get; set; }
        public BudgetInterval BudgetInterval { get; set; }
        public Decimal Amount { get; set; }
    }

}