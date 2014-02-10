using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.Android.Adapters
{
  public class ExpenseWrapper : Java.Lang.Object
  {
    public TextView Name { get; set; }
    public TextView DueDate { get; set; }
    public TextView Total { get; set; }
  }
  public class ExpenseAdapter : BaseAdapter<Expense>
  {
    private ExpensesViewModel viewModel;
    private Activity context;
    public ExpenseAdapter(Activity context, ExpensesViewModel viewModel)
    {
      this.viewModel = viewModel;
      this.context = context;
    }
    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      ExpenseWrapper wrapper = null;
      var view = convertView;
      if (convertView == null)
      {
        view = context.LayoutInflater.Inflate(Resource.Layout.item_expense, null);
        wrapper = new ExpenseWrapper();
        wrapper.Name = view.FindViewById<TextView>(Resource.Id.name);
        wrapper.DueDate = view.FindViewById<TextView>(Resource.Id.due);
        wrapper.Total = view.FindViewById<TextView>(Resource.Id.total);
        view.Tag = wrapper;
      }
      else
      {
        wrapper = convertView.Tag as ExpenseWrapper;
      }

      var expense = viewModel.Expenses[position];
      wrapper.Name.Text = expense.Name;
      wrapper.DueDate.Text = expense.DueDateLongDisplay;
      wrapper.Total.Text = expense.TotalDisplay;

      return view;
    }

    public override Expense this[int position]
    {
      get { return viewModel.Expenses[position]; }
    }

    public override int Count
    {
      get { return viewModel.Expenses.Count; }
    }

    public override long GetItemId(int position)
    {
      return viewModel.Expenses[position].ID;
    }
  }
}