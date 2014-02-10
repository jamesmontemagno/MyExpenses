using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using MyExpenses.Android.Adapters;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.Android.Views
{
  [Activity(Label = "My Expenses", MainLauncher = true, Icon = "@drawable/ic_launcher")]
  public class ExpensesActivity : ListActivity
  {
    private ExpensesViewModel viewModel;
    

    protected async override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.view_expenses);

      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();
      viewModel.IsBusyChanged = (busy) =>
      {
        if(busy)
          AndHUD.Shared.Show(this, "Loading...");
        else
          AndHUD.Shared.Dismiss(this);
      };

      ListAdapter = new ExpenseAdapter(this, viewModel);
    }

    protected async override void OnStart()
    {
      base.OnStart();

      if (!viewModel.LoadedAlert)
      {
        var alert = await viewModel.ExecuteLoadAlert();
        if (alert != null)
        {
          var builder = new AlertDialog.Builder(this);
          builder.SetMessage(alert.Details)
                 .SetTitle(alert.AlertDateDisplay);
          var dialog = builder.Create();
          dialog.Show();
        }
      }

      if (!viewModel.NeedsUpdate)
        return;

      await viewModel.ExecuteLoadExpensesCommand();
      RunOnUiThread(() => ((ExpenseAdapter) ListAdapter).NotifyDataSetChanged());
    }

    protected override void OnListItemClick(ListView l, View v, int position, long id)
    {
      base.OnListItemClick(l, v, position, id);
      var intent = new Intent(this, typeof(ExpenseActivity));
      intent.PutExtra("ID", (int) id);
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
      }
      return base.OnOptionsItemSelected(item);
    }
  }
}

