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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;

namespace MyExpenses.Portable.ViewModels
{
  public class ExpensesViewModel : ViewModelBase
  {
    private IExpenseService expenseService;
    private IMessageDialog messageDialog;

    public ExpensesViewModel()
    {
      expenseService = ServiceContainer.Resolve<IExpenseService>();
      messageDialog = ServiceContainer.Resolve<IMessageDialog>();
      NeedsUpdate = true;
    }

    /// <summary>
    /// Gets or sets if an update is needed
    /// </summary>
    public bool NeedsUpdate { get; set; }


    /// <summary>
    /// Gets or sets if we have loaded alert
    /// </summary>
    public bool LoadedAlert { get; set; }



    private ObservableCollection<Expense> expenses = new ObservableCollection<Expense>();

    public ObservableCollection<Expense> Expenses
    {
      get { return expenses; }
      set { expenses = value; OnPropertyChanged("Expenses"); }
    }


    private async Task UpdateExpenses()
    {
      Expenses.Clear();
      NeedsUpdate = false;
      try
      {
        var exps = await expenseService.GetExpensesAsync();

        foreach (var expense in exps)
        {

          Expenses.Add(expense);
        }

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

    private RelayCommand loadExpensesCommand;

    public ICommand LoadExpensesCommand
    {
      get { return loadExpensesCommand ?? (loadExpensesCommand = new RelayCommand(async () => await ExecuteLoadExpensesCommand())); }
    }

    public async Task ExecuteLoadExpensesCommand()
    {
      if (IsBusy)
        return;

      IsBusy = true;
      await UpdateExpenses();
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

        await expenseService.DeleteExpenseAsync(exp);
        await expenseService.SyncExpensesAsync();
        Expenses.Remove(Expenses.FirstOrDefault(ex => ex.Id == exp.Id));


      }
      catch (Exception ex)
      {
        Debug.WriteLine("Unable to delete expenses");
      }
      finally
      {
        IsBusy = false;
      }
    }

  }
}
