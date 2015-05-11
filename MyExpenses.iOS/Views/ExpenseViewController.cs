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
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using BigTed;
using Microsoft.WindowsAzure.MobileServices;
using MonoTouch.Dialog;
using UIKit;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.iOS.Views
{
  public class ExpenseViewController : DialogViewController
  {
    private EntryElement name, total,notes;
    private CheckboxElement billable;
    private DateElement due;
    private RadioGroup categories;
    private ExpenseViewModel viewModel;
    private Expense expense;
    private IMessageDialog dialog;

    public ExpenseViewController(Expense expense) : base(UITableViewStyle.Plain, null, true)
    {
      this.expense = expense;
      dialog = ServiceContainer.Resolve<IMessageDialog>();
      viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
      viewModel.Init(this.expense);

      viewModel.IsBusyChanged = (busy) =>
      {
        if (busy)
          BTProgressHUD.Show("Saving...");
        else
          BTProgressHUD.Dismiss();
      };
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
      var title = expense == null ? "New Expense" : "Edit Expense";
      this.Root = new RootElement(title)
      {
        new Section("Expense Details")
        {
          (name = new EntryElement("Name", "Expense Name", string.Empty)),
          (total = new EntryElement("Total", "1.00", string.Empty){KeyboardType = UIKeyboardType.DecimalPad}),
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
          (notes = new EntryElement(string.Empty, "Enter expense notes.", string.Empty))
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
        viewModel.Total = total.Value;

        await viewModel.ExecuteSaveExpenseCommand();
        if (!viewModel.CanNavigate)
          return;
        NavigationController.PopToRootViewController(true);
        
      });
    }
  }
}