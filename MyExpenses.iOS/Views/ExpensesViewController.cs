using BigTed;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.iOS.Views
{
  public class ExpensesViewController : UITableViewController
  {
    private ExpensesViewModel viewModel;

    public ExpensesViewController() : base(UITableViewStyle.Plain)
    {
      Title = "My Expenses";
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();

      viewModel.IsBusyChanged = (busy) =>
      {
        if(busy)
          BTProgressHUD.Show("Loading...");
        else
          BTProgressHUD.Dismiss();
      };

      TableView.Source = new ExpensesSource(viewModel, this);
      NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, delegate
      {
        NavigationController.PushViewController(new ExpenseViewController(null), true);
      });
    }

    public async override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      if (!viewModel.LoadedAlert)
      {
        var alert = await viewModel.ExecuteLoadAlert();
        if (alert != null)
        {
          var alertView = new UIAlertView(alert.AlertDateDisplay, alert.Details, null, "OK", null);
          alertView.Show();
        }
      }

      if (!viewModel.NeedsUpdate)
        return;


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
        var expense = viewModel.Expenses[indexPath.Row];
        controller.NavigationController.PushViewController(new ExpenseViewController(expense), true);
      }
    }
  }
}