
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using MyExpenses.Portable.ViewModels;
using MyExpenses.Portable.Helpers;
using MyExpenses.iOS.Views;

namespace MyExpenses.iOS
{
  public partial class ExpenseListViewController : UIViewController
  {
    static bool UserInterfaceIdiomIsPhone
    {
      get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
    }

    private ExpensesViewModel viewModel;
    public ExpenseListViewController(IntPtr handle)
      : base(handle)
    {
    }


    public async override void ViewDidLoad()
    {
      base.ViewDidLoad();
      //NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();

      ExpenseTableView.Source = new ExpenseSource(this, viewModel);

      await viewModel.ExecuteLoadExpensesCommand();
      ExpenseTableView.ReloadData();
    }

    async public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      if(viewModel.NeedsUpdate)
      {
        await viewModel.ExecuteLoadExpensesCommand();
        ExpenseTableView.ReloadData();
      }
    }

    partial void ButtonAdd_TouchUpInside(UIButton sender)
    {
      NavigationController.PushViewController(new ExpenseViewController(null), true);
    }


    public class ExpenseSource : UITableViewSource
    {

      ExpenseListViewController vc;
      ExpensesViewModel viewModel;
      public ExpenseSource(ExpenseListViewController vc, ExpensesViewModel viewModel)
      {
        this.vc = vc;
        this.viewModel = viewModel;
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

      public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
      {
        if (viewModel.IsBusy)
          return;

        var expense = viewModel.Expenses[indexPath.Row];

        vc.NavigationController.PushViewController(new ExpenseViewController(expense), true);
      }
      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
      {
        var cell = tableView.DequeueReusableCell("ExpenseCell");


        var expense = viewModel.Expenses[indexPath.Row];

        cell.TextLabel.Text = expense.Name;
        cell.DetailTextLabel.Text = expense.TotalDisplay;

        return cell;
      }

      public override nint RowsInSection(UITableView tableview, nint section)
      {
        return viewModel.Expenses.Count;
      }

    }

  }
}