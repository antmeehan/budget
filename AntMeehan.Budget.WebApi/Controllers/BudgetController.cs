using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntMeehan.Budget.WebApi
{

    public class BudgetController : Controller
    {
        private readonly BudgetContext _budgetContext;

        public BudgetController (BudgetContext budgetContext)
        {
          _budgetContext = budgetContext;
        }

        [Authorize]
        [Route("api/budget")]
        [HttpGet]
        public IActionResult GetBudget(){
            return Ok(_budgetContext.Budgets.Count());
        }
    }
}