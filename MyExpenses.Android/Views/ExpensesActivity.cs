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
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using MyExpenses.Android.Adapters;
using MyExpenses.PlatformSpecific;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.Android.Views
{
  [Activity(Label = "My Expenses", MainLauncher = true, Icon = "@drawable/ic_launcher")]
  public class ExpensesActivity : ListActivity
  {
    private ExpensesViewModel viewModel;
    private ProgressBar progressBar;
    protected async override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.view_expenses);

      progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();
      viewModel.IsBusyChanged = (busy) =>
      {
        progressBar.Visibility = busy ? ViewStates.Visible : ViewStates.Gone;
      };

      ListAdapter = new ExpenseAdapter(this, viewModel);
     
      ListView.ItemLongClick += async (sender, args) =>
      {
        await viewModel.ExecuteDeleteExpenseCommand(viewModel.Expenses[args.Position]);
        RunOnUiThread(() => ((ExpenseAdapter)ListAdapter).NotifyDataSetChanged());
      };

      if (!viewModel.IsSynced)
      {
        await Authenticate();
        await viewModel.ExecuteSyncExpensesCommand();
        RunOnUiThread(() => ((ExpenseAdapter)ListAdapter).NotifyDataSetChanged());
      }
      
    }

    protected async override void OnStart()
    {
      base.OnStart();
      
      MyExpensesApplication.CurrentActivity = this;

      

      if (viewModel.NeedsUpdate && viewModel.IsSynced)
      {
        await viewModel.ExecuteLoadExpensesCommand();
        RunOnUiThread(() => ((ExpenseAdapter) ListAdapter).NotifyDataSetChanged());
      }
    }

    protected override void OnListItemClick(ListView l, View v, int position, long id)
    {
      base.OnListItemClick(l, v, position, id);
      var intent = new Intent(this, typeof(ExpenseActivity));
      intent.PutExtra("ID",viewModel.Expenses[position].Id);
      StartActivity(intent);
    }

    public override bool OnCreateOptionsMenu(IMenu menu)
    {
      MenuInflater.Inflate(Resource.Menu.menu_expenses, menu);
      return base.OnCreateOptionsMenu(menu);
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
      switch (item.ItemId)
      {
        case (Resource.Id.menu_new_expense):
          var intent = new Intent(this, typeof (ExpenseActivity));
          StartActivity(intent);
          return true;
        case Resource.Id.menu_refresh:
          Sync();
          return true;
      }
      return base.OnOptionsItemSelected(item);
    }

    private async Task Sync()
    {
      await viewModel.ExecuteSyncExpensesCommand();
      RunOnUiThread(() => ((ExpenseAdapter)ListAdapter).NotifyDataSetChanged());
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
          Toast.MakeText(this, message, ToastLength.Long).Show();
        }
      }
    }
  }
}

