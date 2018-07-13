using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApi.Models
{
    public class Category : BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
    }

    
}