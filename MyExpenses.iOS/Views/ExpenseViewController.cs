using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MyExpenses.iOS.Helpers;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.iOS.Views
{
  public class ExpenseViewController : DialogViewController
  {
    private EntryElement name, total;
    private CheckboxElement billable;
    private MultilineEntryElement notes;
    private DateElement due;
    private RadioGroup categories;
    private ExpenseViewModel viewModel;
    private Expense expense;

    public ExpenseViewController(Expense expense) : base(UITableViewStyle.Plain, null, true)
    {
      this.expense = expense;
      viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
      viewModel.Init(this.expense);
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      var title = expense == null ? "New Expense" : "Edit Expense";
      this.Root = new RootElement(title)
      {
        new Section("Expense Details")
        {
          (name = new EntryElement("Name", string.Empty, string.Empty)),
          (total = new EntryElement("Total", string.Empty, string.Empty){KeyboardType = UIKeyboardType.NumbersAndPunctuation}),
          (billable = new CheckboxElement("Billable", true)),
          new RootElement("Category", categories = new RadioGroup("category", 0))
          {
            new Section()
            {
              from category in viewModel.Categories
              select (Element) new RadioElement(category)
            }
          },
          (due = new DateElement("Due Date", DateTime.Now))  
        },
        new Section("Notes")
        {
          (notes = new MultilineEntryElement("Notes", string.Empty, string.Empty))
        } 

      };

      billable.Value = viewModel.Billable;
      name.Value = viewModel.Name;
      total.Value = viewModel.Total;
      notes.Caption = viewModel.Notes;
      categories.Selected = viewModel.Categories.IndexOf(viewModel.Category);
      due.DateValue = viewModel.Due;

      this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Save, async delegate
      {
        viewModel.Category = viewModel.Categories[categories.Selected];
        viewModel.Name = name.Value;
        viewModel.Billable = billable.Value;
        viewModel.Due = due.DateValue;
        viewModel.Notes = notes.Caption;
        await viewModel.ExecuteSaveExpenseCommand();
        NavigationController.PopToRootViewController(true);
      });
    }
  }
}