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
        return Task.FromResult<object>(null);
    }

    public Task UpdateExpenseAsync(Portable.Models.Expense expense)
    {
        return Task.FromResult<object>(null);
    }

    public Task<IEnumerable<Portable.Models.Expense>> GetExpensesAsync()
    {
        return Task.FromResult(Enumerable.Empty<Expense>());
    }
  }
}
