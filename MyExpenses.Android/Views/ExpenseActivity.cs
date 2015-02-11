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
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Microsoft.WindowsAzure.MobileServices;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.ViewModels;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MyExpenses.Android.Views
{
  [Activity(Label = "New Expense", Icon = "@drawable/ic_launcher")]
  public class ExpenseActivity : ActionBarActivity
  {
    private ExpenseViewModel viewModel;
    private EditText notes, name, total;
    private DatePicker date;
    private CheckBox billable;
    private Spinner category;
    private IMessageDialog dialog;
    protected async override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      SetContentView(Resource.Layout.view_expense);
      var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
      //Toolbar will now take on default actionbar characteristics
      SetSupportActionBar(toolbar);
      dialog = ServiceContainer.Resolve<IMessageDialog>();

      var id = Intent.GetStringExtra("ID");
      viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
      await viewModel.Init(id);

      this.SupportActionBar.Title = viewModel.Title;
      viewModel.IsBusyChanged = (busy) =>
      {
        if (busy)
          AndHUD.Shared.Show(this, "Saving...");
        else
          AndHUD.Shared.Dismiss(this);
      };

      name = FindViewById<EditText>(Resource.Id.name);
      date = FindViewById<DatePicker>(Resource.Id.date);
      notes = FindViewById<EditText>(Resource.Id.notes);
      total = FindViewById<EditText>(Resource.Id.total);
      billable = FindViewById<CheckBox>(Resource.Id.billable);
      category = FindViewById<Spinner>(Resource.Id.category);
      category.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerDropDownItem, viewModel.Categories);
      category.SetSelection(viewModel.Categories.IndexOf(viewModel.Category));
      name.Text = viewModel.Name;
      date.DateTime = viewModel.Due;
      notes.Text = viewModel.Notes;
      total.Text = viewModel.Total;
      billable.Checked = viewModel.Billable;
    }

    protected override void OnStart()
    {
      base.OnStart();
      MyExpensesApplication.CurrentActivity = this;
    }

    public override bool OnCreateOptionsMenu(IMenu menu)
    {
      MenuInflater.Inflate(Resource.Menu.menu_expense, menu);
      return base.OnCreateOptionsMenu(menu);
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
      switch (item.ItemId)
      {
        case (Resource.Id.menu_save_expense):
          viewModel.Name = name.Text;
          viewModel.Billable = billable.Checked;
          viewModel.Due = date.DateTime;
          viewModel.Notes = notes.Text;
          viewModel.Total = total.Text;
          viewModel.Category = viewModel.Categories[category.SelectedItemPosition];
          Task.Run(async () =>
          {
              await viewModel.ExecuteSaveExpenseCommand();

              if (!viewModel.CanNavigate)
                return;

              Finish();
          });
          return true;
      }
      return base.OnOptionsItemSelected(item);
    }
  }
}