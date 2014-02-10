using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExpenses.Portable.Models;

namespace MyExpenses.Portable.Interfaces
{
  public interface IExpenseService
  {
    Task<Expense> GetExpense(int id);
    Task<IEnumerable<Expense>> GetExpenses();
    Task<Expense> SaveExpense(Expense expense);
    Task<int> DeleteExpense(int id);

  }
}
