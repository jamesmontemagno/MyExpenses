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

    Task<Expense> GetExpenseAsync(string id);
    Task<IEnumerable<Expense>> GetExpensesAsync();
    Task SyncExpensesAsync();
    Task<Expense> SaveExpenseAsync(Expense expense);
    Task<string> DeleteExpenseAsync(Expense expense);
    string UserId { get; }

    Task Init();

  }
}
