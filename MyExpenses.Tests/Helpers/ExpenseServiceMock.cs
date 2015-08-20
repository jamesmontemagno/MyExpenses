//
//  Copyright 2014  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.using System;
using System.Collections;
using MyExpenses.Portable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExpenses.Portable.Models;

namespace MyExpenses.Tests.Helpers
{
  public class ExpenseServiceMock : IExpenseService
  {
    public List<Expense> Expenses = new List<Expense>(); 
    public Task<Portable.Models.Expense> GetExpense(int id)
    {
      return Task.Run(()=>Expenses.FirstOrDefault(e => e.ID == id));
    }

    public Task<IEnumerable<Portable.Models.Expense>> GetExpenses()
    {
        return Task.FromResult<IEnumerable<Expense>>(Expenses);
    }

    public async Task<Portable.Models.Expense> SaveExpense(Portable.Models.Expense expense)
    {

      var ex = await GetExpense(expense.ID);
      if (ex == null)
      {
        expense.ID = Expenses.Count;
        Expenses.Add(expense);
      }
      else
      {
        Expenses.Remove(ex);
        Expenses.Add(expense);
      }
      return expense;
    }

    public async Task<int> DeleteExpense(int id)
    {
      var ex = await GetExpense(id);
      if (ex != null)
      {
        Expenses.Remove(ex);
      }

      return 0;
    }


    public Task<Alert> GetExpenseAlert()
    {
      throw new NotImplementedException();
    }
  }
}
