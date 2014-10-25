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
using Android.App;
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
      return position;
    }
  }
}