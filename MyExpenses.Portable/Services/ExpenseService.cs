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
using System.Threading.Tasks;
using MyExpenses.Portable.DataLayer;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using Newtonsoft.Json;
using SQLite.Net;

namespace MyExpenses.Portable.Services
{
  public class ExpenseService : IExpenseService
  {

    ExpenseDatabase db = null;
    private readonly ICloudService azureService;

    private readonly IMessageDialog dialog;

    public ExpenseService(SQLiteConnection conn)
    {
      db = new ExpenseDatabase(conn);

      azureService = ServiceContainer.Resolve<ICloudService>();
      dialog = ServiceContainer.Resolve<IMessageDialog>();
    }

    public Task<Expense> GetExpense(int id)
    {
      return Task.Factory.StartNew(() => db.GetItem<Expense>(id));
    }

    public Task<IEnumerable<Expense>> GetExpenses()
    {
      return Task.Factory.StartNew(() => db.GetVisibleItems());
    }

    public async Task<Expense> SaveExpense(Expense item)
    {
      
      try
      {
        item.IsDirty = false;
        //if it is new then inser record, else update!
        if (string.IsNullOrWhiteSpace(item.AzureId))
        {
          await azureService.InsertExpenseAsync(item);
        }
        else
        {
          await azureService.UpdateExpenseAsync(item);
        }
      }
      catch (Exception ex)
      {
        item.IsDirty = true;
        //unable to sync
        dialog.SendMessage(ex.Message, "Problem!");
      }

      return await Task.Factory.StartNew(() =>
       {

         var id = db.SaveItem<Expense>(item);
         item.Id = id;
         return item;
       });
    }

    public async Task<int> DeleteExpense(int id)
    {
      var item = db.GetItem<Expense>(id);
      item.IsVisible = false;

      var updatedItem = await SaveExpense(item);
      return updatedItem.Id;
    }

    /// <summary>
    /// Returns true always
    /// </summary>
    /// <returns></returns>
    public async Task<bool> SyncExpenses()
    {
      try
      {
        var expenses = await GetExpenses();
        //syncs if we were offline
        foreach (var item in expenses)
        {
          //new item added while offline
          if (string.IsNullOrWhiteSpace(item.AzureId))
          {
            await azureService.InsertExpenseAsync(item);
            item.IsDirty = false;
            db.SaveItem(item);
          }
          else if (item.IsDirty) //updated while offline
          {
            await azureService.UpdateExpenseAsync(item);
            item.IsDirty = false;
            db.SaveItem(item);
          }
        }

        //sync from online.
        var items = await azureService.GetExpensesAsync();
        foreach (var expense in items)
        {
          var item = db.GetItem(expense.AzureId);
          if (item == null)
          {
            item = new Expense(expense);
          }
          else
          {
            item.SyncProperties(expense);
          }
          db.SaveItem(item);
        }
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
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

