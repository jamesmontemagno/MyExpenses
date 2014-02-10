using System;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
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
      if (!viewModel.LoadedAlert)
      {
        var alert = await viewModel.ExecuteLoadAlert();
        if (alert != null)
        {
          MessageBox.Show(alert.Details, alert.AlertDateDisplay, MessageBoxButton.OK);
        }
      }

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