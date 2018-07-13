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
    public class UsersController : ControllerBase
    {
        private readonly UserContext userContext;

        private readonly CategoryContext categoryContext;

        public UsersController(UserContext _context)
        {
            userContext = _context;

            if (userContext.Users.Count() == 0)
            {
                userContext.Users.Add(new User
                {
                    Name = "user",
                    Email = "user@users.com",
                    Password = "password"
                });
                userContext.Transactions.AddRange(
                    new Transaction { UserID = 1, CategoryID = 1, Amount = 3.50m, Description = "Chicken McNuggets", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 1, Amount = 13.67m, Description = "Moar Chicken McNuggets", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 1, Amount = 153.67m, Description = "All the Chicken McNuggets", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 2, Amount = 25.00m, Description = "Boogie Fever", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 2, Amount = 17.76m, Description = "Scorcher", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 2, Amount = 17.76m, Description = "Scorcher II", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 2, Amount = 17.76m, Description = "Scorcher III", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 2, Amount = 17.76m, Description = "Scorcher IV", DateOfPurchase = DateTime.Now },
                    new Transaction { UserID = 1, CategoryID = 2, Amount = 17.76m, Description = "Scorcher V", DateOfPurchase = DateTime.Now }
                    );
                userContext.CategoryBudgets.AddRange(
                    new CategoryBudget { UserID = 1, CategoryID = 1, BudgetInterval = BudgetInterval.WEEKLY, Amount = 20.00m },
                    new CategoryBudget { UserID = 1, CategoryID = 1, BudgetInterval = BudgetInterval.MONTHLY, Amount = 40.00m },
                    new CategoryBudget { UserID = 1, CategoryID = 2, BudgetInterval = BudgetInterval.WEEKLY, Amount = 20.00m },
                    new CategoryBudget { UserID = 1, CategoryID = 2, BudgetInterval = BudgetInterval.MONTHLY, Amount = 40.00m }
                );

                userContext.SaveChanges();
            }
        }

        // GET api/users
        [HttpGet(Name = "GetUsers")]
        public ActionResult<IEnumerable<User>> GetUsers(string username, string email)
        {
            var users = from q in userContext.Users
                        select q;

            if (!String.IsNullOrEmpty(username))
            {
                users = users.Where(q => q.Name.Equals(username));
            }

            if (!String.IsNullOrEmpty(email))
            {
                users = users.Where(q => q.Email.Equals(email));
            }

            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return users.ToList();


        }

        // GET api/users/5
        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetUser(long id)
        {
            var user = userContext.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;

        }

        // GET api/users/{id}/categoryBudgets
        [HttpGet("{id}/categoryBudgets", Name = "GetCategoryBudgets")]
        public ActionResult<IEnumerable<CategoryBudget>> GetCategoryBudgets(long id)
        {
            var categoryBudgets = from q in userContext.CategoryBudgets
                                  select q;

            categoryBudgets = categoryBudgets.Where(q => q.UserID.Equals(id));

            if (categoryBudgets == null || !categoryBudgets.Any())
            {
                return NotFound();
            }

            return categoryBudgets.ToList();
        }

        // GET api/users/{id}/categoryBudgets/{categoryBudgetID}
        [HttpGet("{id}/categoryBudgets/{categoryBudgetID}", Name = "GetCategoryBudget")]
        public ActionResult<CategoryBudget> GetCategoryBudget(long id, long categoryBudgetID)
        {
            var categoryBudgets = from q in userContext.CategoryBudgets
                                  select q;

            categoryBudgets = categoryBudgets.Where(q => q.UserID.Equals(id));
            categoryBudgets = categoryBudgets.Where(q => q.Id.Equals(categoryBudgetID));

            if (categoryBudgets == null || !categoryBudgets.Any())
            {
                return NotFound();
            }

            return categoryBudgets.First();
        }

        [HttpGet("{id}/transactions", Name = "GetTransactions")]
        public ActionResult<IEnumerable<Transaction>> GetTransactions(long id, long? categoryID, DateTime purchaseDateStart, DateTime purchaseDateEnd)
        {
            var trans = from q in userContext.Transactions
                        select q;


            trans = trans.Where(q => q.UserID.Equals(id));

            if (categoryID != null && categoryID != 0) {
                trans = trans.Where(q => q.CategoryID == categoryID);
            }

            if (purchaseDateStart != null && purchaseDateStart != DateTime.MinValue) {
                trans = trans.Where(q => q.DateOfPurchase > purchaseDateStart);
            }

            
            if (purchaseDateEnd != null && purchaseDateEnd != DateTime.MinValue) {
                trans = trans.Where(q => q.DateOfPurchase < purchaseDateEnd);
            }

            if (trans == null || !trans.Any())
            {
                return NotFound();
            }

            return trans.ToList();
        }

        [HttpGet("{id}/transactions/{transactionID}", Name = "GetTransaction")]
        public ActionResult<Transaction> GetTransaction(long id, long transactionID)
        {
            var transactions = from q in userContext.Transactions
                               select q;

            transactions = transactions.Where(q => q.UserID.Equals(id));
            transactions = transactions.Where(q => q.Id.Equals(transactionID));

            if (transactions == null || !transactions.Any())
            {
                return NotFound();
            }

            return transactions.First();
        }

        // POST api/users
        [HttpPost(Name = "PostUser")]
        public ActionResult PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            userContext.Users.Add(user);
            userContext.SaveChanges();

            return CreatedAtRoute(
                routeName: "GetUser",
                routeValues: new { id = user.Id },
                value: user);
        }

        [HttpPost("{id}/categoryBudgets", Name = "PostCategoryBudget")]
        public ActionResult PostCategoryBudget(long id, [FromBody] CategoryBudget categoryBudget)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var categoryBudgets = from q in userContext.CategoryBudgets select q;
            categoryBudgets = categoryBudgets.Where(q => q.UserID.Equals(id))
                .Where(q => q.CategoryID.Equals(categoryBudget.CategoryID))
                .Where(q => q.BudgetInterval.Equals(categoryBudget.BudgetInterval));

            if (categoryBudgets != null && categoryBudgets.Any() ) {
                return BadRequest("Request is invalid");
            }
            categoryBudget.UserID = id;

            userContext.CategoryBudgets.Add(categoryBudget);
            userContext.SaveChanges();

            return CreatedAtRoute(
                routeName: "GetCategoryBudget",
                routeValues: new { id = categoryBudget.Id },
                value: categoryBudget
            );
        }

        
        [HttpPost("{id}/transactions", Name = "PostTransction")]
        public ActionResult PostTransction(long id, [FromBody] Transaction transaction)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            transaction.UserID = id;

            userContext.Transactions.Add(transaction);
            userContext.SaveChanges();

            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}", Name = "PutUser")]
        public ActionResult PutUser(long id, [FromBody] User updateUser)
        {
            var user = userContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = updateUser.Name;
            user.Email = updateUser.Email;
            user.Password = updateUser.Password;

            userContext.Users.Update(user);
            userContext.SaveChanges();
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}/categoryBudgets/{categoryBudgetID}", Name = "PutCategoryBudget")]
        public ActionResult PutCategoryBudget(long id, long categoryBudgetID, [FromBody] CategoryBudget updateCB)
        {
            var categoryBudgets = from q in userContext.CategoryBudgets
                                  select q;

            categoryBudgets = categoryBudgets.Where(q => q.UserID.Equals(id));
            categoryBudgets = categoryBudgets.Where(q => q.Id.Equals(categoryBudgetID));

            if (categoryBudgets == null || !categoryBudgets.Any())
            {
                return NotFound();
            }

            var cb = categoryBudgets.First();

            cb.UserID = updateCB.UserID;
            cb.CategoryID = updateCB.CategoryID;
            cb.BudgetInterval = updateCB.BudgetInterval;

            userContext.CategoryBudgets.Update(cb);
            userContext.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}/transactions/{transactionID}", Name = "PutTransaction")]
        public ActionResult PutTransaction(long id, long transactionID, [FromBody] Transaction updateTrans)
        {
            var trans = from q in userContext.Transactions
                        select q;

            trans = trans.Where(q => q.UserID.Equals(id));
            trans = trans.Where(q => q.Id.Equals(transactionID));

            if (trans == null || !trans.Any())
            {
                return NotFound();
            }

            var t = trans.First();

            t.UserID = updateTrans.UserID;
            t.CategoryID = updateTrans.CategoryID;
            t.Amount = updateTrans.Amount;
            t.Description = updateTrans.Description;

            userContext.Transactions.Update(t);
            userContext.SaveChanges();
            return Ok();
        }



        // DELETE api/users/5
        [HttpDelete("{id}", Name = "DeleteUser")]
        public ActionResult DeleteUser(long id)
        {

            User user = userContext.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            // all related values that are dependent on a user must be deleted to keep the relation normal
            var categoryBudgets = from q in userContext.CategoryBudgets select q;
            categoryBudgets = categoryBudgets.Where(q => q.UserID.Equals(id));

            var transactions = from q in userContext.Transactions select q;
            transactions = transactions.Where(q => q.UserID.Equals(id));

            userContext.CategoryBudgets.RemoveRange(categoryBudgets);
            userContext.Transactions.RemoveRange(transactions);

            userContext.Users.Remove(user);
            userContext.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}/transactions/{transactionID}", Name = "DeleteTransaction")]
        public ActionResult DeleteTransaction(long id, long transactionID)
        {
            var trans = from q in userContext.Transactions
                        select q;

            trans = trans.Where(q => q.UserID.Equals(id));
            trans = trans.Where(q => q.Id.Equals(transactionID));

            if (trans == null || !trans.Any())
            {
                NotFound();
            }

            userContext.Transactions.Remove(trans.FirstOrDefault());
            userContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}/categoryBudgets/{categoryBudgetID}", Name = "DeleteCategoryBudget")]
        public ActionResult DeleteCategoryBudget(long id, long categoryBudgetID)
        {
            var cb = from q in userContext.CategoryBudgets
                     select q;

            cb = cb.Where(q => q.UserID.Equals(id));
            cb = cb.Where(q => q.Id.Equals(categoryBudgetID));

            if (cb == null || !cb.Any())
            {
                NotFound();
            }

            userContext.CategoryBudgets.Remove(cb.First());
            userContext.SaveChanges();
            return Ok();
        }
    }
}
