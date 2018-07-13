using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApi.Models
{

    public class Transaction : BaseModel
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }

        public DateTime DateOfPurchase { get; set; }


    }
    
}