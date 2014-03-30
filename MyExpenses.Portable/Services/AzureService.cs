
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;

namespace MyExpenses.PlatformSpecific
{
  public class AzureService : ICloudService
  {
    public MobileServiceClient MobileService { get; set; }

    private readonly IMobileServiceTable<Expense> expenseTable;

    public AzureService()
    {
      //comment back in to enable Azure Mobile Services.
      /*MobileService = new MobileServiceClient(
                    "https://" + "PUT-SITE-HERE" + ".azure-mobile.net/",
                    "PUT-YOUR-API-KEY-HERE");


      
      expenseTable = MobileService.GetTable<Expense>();*/
    }
    
    
    public Task InsertExpenseAsync(Expense expense)
    {
      if (expenseTable == null)
        return Task.Factory.StartNew(() => { expense.UserId = UserId; });
      
      return expenseTable.InsertAsync(expense);
    }

    public Task UpdateExpenseAsync(Expense expense)
    {
      if (expenseTable == null)
        return Task.Factory.StartNew(() => { expense.UserId = UserId; });

      return expenseTable.UpdateAsync(expense);
    }

    public Task<IEnumerable<Expense>> GetExpensesAsync()
    {
      if (expenseTable == null)
        return Task<IEnumerable<Expense>>.Factory.StartNew(() => { return new List<Expense>(); });


      return expenseTable.ToEnumerableAsync();
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

    static readonly AzureService instance = new AzureService();
    /// <summary>
    /// Gets the instance of the Azure Web Service
    /// </summary>
    public static AzureService Instance
    {
      get
      {
        return instance;
      }
    }
  }
}