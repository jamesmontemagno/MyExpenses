using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

