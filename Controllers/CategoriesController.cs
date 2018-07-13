using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinanceApi.Models;
using FinanceApi.DataAccess;

namespace FinanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryContext context;
        private readonly UserContext userContext;

        public CategoriesController(CategoryContext _context)
        {
            context = _context;

            if (context.Categories.Count() == 0)
            {
                context.Categories.Add(new Category
                {
                    Name = "Groceries"
                });
                context.Categories.Add(new Category
                {
                    Name = "Entertainment"
                });
                context.SaveChanges();
            }
        }

        // GET api/Categories
        [HttpGet(Name = "GetCategories")]
        public ActionResult<IEnumerable<Category>> GetCategories(string categoryName)
        {
            var cats = from q in context.Categories
            select q;

            if (!String.IsNullOrEmpty(categoryName))
            {
                cats = cats.Where(q => q.Name.Equals(categoryName));
            }

            if (cats == null || !cats.Any()) {
                return NotFound();
            }

            return cats.ToList();
        }

        // GET api/Categories/5
        [HttpGet("{id}", Name = "GetCategory")]
        public ActionResult<Category> GetCategory(long id)
        {
            var category = context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;

        }

        // POST api/Categories
        [HttpPost(Name = "PostCategory")]
        public ActionResult PostCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var cats = from q in context.Categories
                select q;

            cats = cats.Where(q => q.Name.Equals(category.Name));

            if (cats != null && cats.Any()) {
               return BadRequest("Duplicate category name");
            }

            context.Categories.Add(category);
            context.SaveChanges();

            return CreatedAtRoute(
                routeName: "GetCategory",
                routeValues: new { id = category.Id },
                value: category);
        }

        // PUT api/values/5
        [HttpPut("{id}", Name = "PutCategory")]
        public ActionResult PutCategory(long id, [FromBody] Category updateCategory)
        {
            var category = context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = updateCategory.Name;

            context.Categories.Update(category);
            context.SaveChanges();
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}", Name = "DeleteCategory")]
        public ActionResult DeleteCategory(long id)
        {

            Category category = context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryBudgets = from q in userContext.CategoryBudgets select q;
            categoryBudgets = categoryBudgets.Where(q => q.CategoryID.Equals(id));

            var transactions = from q in userContext.Transactions select q;
            transactions = transactions.Where(q => q.CategoryID.Equals(id));

            userContext.CategoryBudgets.RemoveRange(categoryBudgets);
            userContext.Transactions.RemoveRange(transactions);
            context.Categories.Remove(category);
            userContext.SaveChanges();
            context.SaveChanges();

            return Ok();

        }
    }
}
