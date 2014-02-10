
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using Newtonsoft.Json;

namespace MyExpenses.Portable.ViewModels
{
  public class ExpensesViewModel : ViewModelBase
  {
    private IExpenseService expenseService;
    /// <summary>
    /// Gets or sets if an update is needed
    /// </summary>
    public bool NeedsUpdate { get; set; }
    /// <summary>
    /// Gets or sets if we have loaded alert
    /// </summary>
    public bool LoadedAlert { get; set; }
    public ExpensesViewModel()
    {
      expenseService = ServiceContainer.Resolve<IExpenseService>();
      NeedsUpdate = true;
    }
    public ExpensesViewModel(IExpenseService expenseService)
    {
      this.expenseService = expenseService;
      NeedsUpdate = true;
    }

    private ObservableCollection<Expense> expenses = new ObservableCollection<Expense>();

    public ObservableCollection<Expense> Expenses
    {
      get { return expenses; }
      set { expenses = value; OnPropertyChanged("Expenses"); }
    } 

    private RelayCommand loadExpensesCommand;

    public ICommand LoadExpensesCommand
    {
      get { return loadExpensesCommand ?? (loadExpensesCommand = new RelayCommand(async () => await ExecuteLoadExpensesCommand())); }
    }

    public async Task ExecuteLoadExpensesCommand()
    {
      if (IsBusy)
        return;

      Expenses.Clear();
      IsBusy = true;
      NeedsUpdate = false;
      try
      {
        await Task.Delay(5000);
        var exps = await expenseService.GetExpenses();
        foreach (var expense in exps)
          Expenses.Add(expense);

      }
      catch (Exception exception)
      {
        Debug.WriteLine("Unable to query and gather expenses");
      }
      finally
      {
        IsBusy = false;
      }
    }

    private RelayCommand<Expense> deleteExpensesCommand;

    public ICommand DeleteExpenseCommand
    {
      get { return deleteExpensesCommand ?? (deleteExpensesCommand = new RelayCommand<Expense>(async (item) => await ExecuteDeleteExpenseCommand(item))); }
    }

    public async Task ExecuteDeleteExpenseCommand(Expense exp)
    {
      if (IsBusy)
        return;

      IsBusy = true;
      try
      {

        await expenseService.DeleteExpense(exp.ID);
        Expenses.Remove(Expenses.FirstOrDefault(ex => ex.ID == exp.ID));


      }
      catch (Exception)
      {
        Debug.WriteLine("Unable to delete expenses");
      }
      finally
      {
        IsBusy = false;
      }
    }

  
    /// <summary>
    /// Gets the current expense alert from the server
    /// </summary>
    /// <returns>Alert from server</returns>
    public async Task<Alert> ExecuteLoadAlert()
    {
      if (IsBusy)
        return null;

      LoadedAlert = true;
      try
      {
        var client = new HttpClient();
        client.Timeout = new TimeSpan(0,0,0,5);

        var response = await client.GetStringAsync("https://gist.github.com/jamesmontemagno/a54af53e027308362415/raw/a828b194254b241281aad79cd362c33295fdb183/gistfile1.txt");
        return await ExpenseService.DeserializeObjectAsync<Alert>(response);

      }
      catch (Exception exception)
      {
        Debug.WriteLine("Unable to query and gather expenses");
      }
      finally
      {
        IsBusy = false;
      }

      return null;
    }
  }
}
