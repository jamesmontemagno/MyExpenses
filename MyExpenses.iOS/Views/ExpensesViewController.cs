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
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MyExpenses.PlatformSpecific;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.iOS.Views
{
  public class ExpensesViewController : UITableViewController
  {
    private ExpensesViewModel viewModel;

    public ExpensesViewController()
      : base(UITableViewStyle.Plain)
    {
      Title = "My Expenses";
    }

    public async override void ViewDidLoad()
    {
      base.ViewDidLoad();

      NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();

      viewModel.IsBusyChanged = (busy) =>
      {
        if (busy)
          RefreshControl.BeginRefreshing();
        else
          RefreshControl.EndRefreshing();
      };


      this.RefreshControl = new UIRefreshControl();

      RefreshControl.ValueChanged += async (sender, args) =>
      {
        if (viewModel.IsBusy)
          return;

        await viewModel.ExecuteLoadExpensesCommand();
        TableView.ReloadData();
      };

      TableView.Source = new ExpensesSource(viewModel, this);
      NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, delegate
      {
        NavigationController.PushViewController(new ExpenseViewController(null), true);
      });

      await Authenticate();
      await viewModel.ExecuteLoadExpensesCommand();
      TableView.ReloadData();
    }


    public class ExpensesSource : UITableViewSource
    {
      private ExpensesViewModel viewModel;
      private string cellIdentifier = "ExpenseCell";
      private ExpensesViewController controller;
      public ExpensesSource(ExpensesViewModel viewModel, ExpensesViewController controller)
      {
        this.viewModel = viewModel;
        this.controller = controller;
      }

      public override int RowsInSection(UITableView tableview, int section)
      {
        return viewModel.Expenses.Count;
      }

      public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
      {
        return UITableViewCellEditingStyle.Delete;
      }

      public async override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
      {
        var expense = viewModel.Expenses[indexPath.Row];
        await viewModel.ExecuteDeleteExpenseCommand(expense);
        tableView.ReloadData();
      }

      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
      {
        var cell = tableView.DequeueReusableCell(cellIdentifier);
        if (cell == null)
        {
          cell = new UITableViewCell(UITableViewCellStyle.Value2, cellIdentifier);
          cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
        }

        var expense = viewModel.Expenses[indexPath.Row];
        cell.DetailTextLabel.Text = expense.TotalDisplay + ": " + expense.Name;
        cell.TextLabel.Text = expense.DueDateShortDisplay;

        return cell;
      }

      public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
      {
        if (viewModel.IsBusy)
          return;

        var expense = viewModel.Expenses[indexPath.Row];

        controller.NavigationController.PushViewController(new ExpenseViewController(expense), true);
      }
    }


    /// <summary>
    /// Authenticate the azure client with twitter authentication.
    /// </summary>
    /// <returns></returns>
    private async Task Authenticate()
    {
      return;
      var client = AzureExpenseService.Instance.MobileService;
      if (client == null)
        return;

      while (client.CurrentUser == null)
      {
        try
        {
          client.CurrentUser = await client
            .LoginAsync(this, MobileServiceAuthenticationProvider.Twitter);
        }
        catch (InvalidOperationException ex)
        {
          var message = "You must log in. Login Required";
          var alert = new UIAlertView("Login", message, null, "OK", null);
          alert.Show();
        }
      }
    }

    public async override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      if (viewModel.NeedsUpdate)
      {
        await viewModel.ExecuteLoadExpensesCommand();
        TableView.ReloadData();
      }
    }

  }
}