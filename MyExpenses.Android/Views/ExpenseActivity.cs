using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.Android.Views
{
  [Activity(Label = "New Expense", Icon = "@drawable/ic_launcher")]
  public class ExpenseActivity : Activity
  {
    private ExpenseViewModel viewModel;
    private EditText notes, name;
    private DatePicker date;
    private CheckBox billable;
    private Spinner category;
    protected async override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      SetContentView(Resource.Layout.view_expense);

      var id = Intent.GetIntExtra("ID", -1);
      if (id != -1)
        this.ActionBar.Title = "Edit Expense";

      viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
      await viewModel.Init(id);
      viewModel.IsBusyChanged = (busy) =>
      {
        if (busy)
          AndHUD.Shared.Show(this, "Loading...");
        else
          AndHUD.Shared.Dismiss(this);
      };

      name = FindViewById<EditText>(Resource.Id.name);
      date = FindViewById<DatePicker>(Resource.Id.date);
      notes = FindViewById<EditText>(Resource.Id.notes);
      billable = FindViewById<CheckBox>(Resource.Id.billable);
      category = FindViewById<Spinner>(Resource.Id.category);
      category.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerDropDownItem, viewModel.Categories);
      category.SetSelection(viewModel.Categories.IndexOf(viewModel.Category));
      name.Text = viewModel.Name;
      date.DateTime = viewModel.Due;
      notes.Text = viewModel.Notes;
      billable.Checked = viewModel.Billable;
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
          viewModel.Category = viewModel.Categories[category.SelectedItemPosition];
          Task.Run(async () =>
          {
            await viewModel.ExecuteSaveExpenseCommand();
            Finish();
          });
          return true;
      }
      return base.OnOptionsItemSelected(item);
    }
  }
}