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
using System.Collections.Generic;
using System.Threading.Tasks;
using MyExpenses.Portable.DataLayer;
using MyExpenses.Portable.DataLayer.SQLiteBase;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using Newtonsoft.Json;

namespace MyExpenses.Portable.Services
{
  public class ExpenseService : IExpenseService
  {
    ExpenseDatabase db = null;
    protected static string dbLocation;

    public ExpenseService(SQLiteConnection conn)
    {
      db = new ExpenseDatabase(conn);
    }

    public Task<Expense> GetExpense(int id)
    {
      return Task.Factory.StartNew(() => db.GetItem<Expense>(id));

    }

    public Task<IEnumerable<Expense>> GetExpenses()
    {
       return Task.Factory.StartNew(() => db.GetItems<Expense>());
    }

    public Task<Expense> SaveExpense(Expense item)
    {
       return Task.Factory.StartNew(() =>
       {
         var id = db.SaveItem<Expense>(item);
         item.ID = id;
         return item;
       });
    }

    public Task<int> DeleteExpense(int id)
    {
       return Task.Factory.StartNew(() => db.DeleteItem<Expense>(id));
    }


    public static Task<T> DeserializeObjectAsync<T>(string value)
    {
      return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));
    }

    public static T DeserializeObject<T>(string value)
    {
      return JsonConvert.DeserializeObject<T>(value);
    }

  }
}

