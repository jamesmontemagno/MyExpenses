
using System.Collections.Generic;
using System.Threading.Tasks;
using MyExpenses.Portable.Models;

namespace MyExpenses.Portable.Interfaces
{
  public interface ICloudService
  {
    Task InsertExpenseAsync(Expense expense);
    Task UpdateExpenseAsync(Expense expense);

    Task<IEnumerable<Expense>> GetExpensesAsync();

    string UserId { get; }

  }
}
