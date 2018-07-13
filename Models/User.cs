using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApi.Models
{
    public class User : BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<CategoryBudget> CategoryBudgets { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
    
}