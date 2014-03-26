using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;

namespace MyExpenses.Tests.Helpers
{
  public class CloudServiceMock : ICloudService
  {
    public Task InsertExpenseAsync(Portable.Models.Expense expense)
    {
      return new Task(() => { });
    }

    public Task UpdateExpenseAsync(Portable.Models.Expense expense)
    {
      return new Task(() => { });
    }

    public Task<IEnumerable<Portable.Models.Expense>> GetExpensesAsync()
    {
      return new Task<IEnumerable<Expense>>(()=> new List<Expense>());
    }
  }
}
