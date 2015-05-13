
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using System;

namespace MyExpenses.PlatformSpecific
{
  public class AzureExpenseService : IExpenseService
  {
    public MobileServiceClient MobileService { get; set; }

    private IMobileServiceSyncTable<Expense> expenseTable;

    public AzureExpenseService()
    {
     //comment back in to enable Azure Mobile Services.
     MobileService = new MobileServiceClient(
       "https://" + "YOUR-SITE" + ".azure-mobile.net/",
       "YOUR-API-KEY");
    }

    
    public async Task Init()
    {
      string path = "syncstore3.db";
			var store = new MobileServiceSQLiteStore (path);
			store.DefineTable<Expense> ();
			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

      expenseTable = MobileService.GetSyncTable<Expense>();
    }
    
    
    public async Task<Expense> SaveExpenseAsync(Expense expense)
    {
      if (expenseTable == null)
      {
        expense.UserId = UserId; 
        return expense; 
      };

      if (string.IsNullOrWhiteSpace(expense.Id))
        await expenseTable.InsertAsync(expense);
      else
        await expenseTable.UpdateAsync(expense);
      return expense; 
    }


    public async Task<string> DeleteExpenseAsync(Expense expense)
    {
      if (expenseTable == null)
        return null;
      await expenseTable.DeleteAsync(expense);
      return expense.Id;
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync()
    {
      if (expenseTable == null)
        return new List<Expense>();

      await SyncExpensesAsync();
      return await expenseTable.ToEnumerableAsync();
    }

    public async Task<Expense> GetExpenseAsync(string id)
    {
      if (expenseTable == null)
        return null;

     
      var expenses = await expenseTable.Where(s => s.Id == id).ToEnumerableAsync();
      return expenses.Count() == 0 ? null : expenses.ElementAt(0);
    }

    public async Task SyncExpensesAsync()
    {
      try
      {
        await MobileService.SyncContext.PushAsync();
        await expenseTable.PullAsync("allItems", expenseTable.CreateQuery());
      }
      catch (MobileServiceInvalidOperationException e)
      {
        Debug.WriteLine(@"Sync Failed: {0}", e.Message);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(@"Sync Failed: {0}", ex.Message);
     
      }
    }

    public string UserId
    {
      get 
      {
        if (MobileService == null || MobileService.CurrentUser == null)
          return string.Empty;

        return MobileService.CurrentUser.UserId; 
      }
    }

    static readonly AzureExpenseService instance = new AzureExpenseService();
    /// <summary>
    /// Gets the instance of the Azure Web Service
    /// </summary>
    public static AzureExpenseService Instance
    {
      get
      {
        return instance;
      }
    }
  }
}
