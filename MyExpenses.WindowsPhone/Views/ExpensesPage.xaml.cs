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
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.WindowsPhone.Views
{
  public partial class ExpensesPage : PhoneApplicationPage
  {
    private ExpensesViewModel viewModel;
    // Constructor
    public ExpensesPage()
    {
      InitializeComponent();

      // Set the data context of the LongListSelector control to the sample data
      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();
      DataContext = viewModel;
    }

    // Load data for the ViewModel Items
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
      if(viewModel.NeedsUpdate)
        viewModel.LoadExpensesCommand.Execute(null);
    }

    // Handle selection changed on LongListSelector
    private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // If selected item is null (no selection) do nothing
      if (MainLongListSelector.SelectedItem == null)
        return;

      // Navigate to the new page
      NavigationService.Navigate(new Uri("/Views/ExpensePage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as Expense).ID, UriKind.Relative));

      // Reset selected item to null (no selection)
      MainLongListSelector.SelectedItem = null;
    }


    private void Delete_OnClick(object sender, RoutedEventArgs e)
    {
      var menuItem = sender as MenuItem;

      if (menuItem != null)
      {
        var selected = menuItem.DataContext as Expense;
        if (selected == null)
          return;

        viewModel.DeleteExpenseCommand.Execute(selected);
      }
    }

    private void NewExpenseAppButton_OnClick(object sender, EventArgs e)
    {
      // Navigate to the new page
      NavigationService.Navigate(new Uri("/Views/ExpensePage.xaml", UriKind.Relative));

    }
  }
}