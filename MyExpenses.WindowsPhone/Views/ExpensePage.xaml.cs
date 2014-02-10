using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;
using MyExpenses.WindowsPhone.Resources;

namespace MyExpenses.WindowsPhone
{
  public partial class ExpensePage : PhoneApplicationPage
  {
    private ExpenseViewModel viewModel;
    // Constructor
    public ExpensePage()
    {
      InitializeComponent();
    }

    // When page is navigated to set data context to selected item in list
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (DataContext == null)
      {
        string selectedIndex = "";
        int id = -1;
        viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
        if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
        {
          id = int.Parse(selectedIndex);
        }
        viewModel.Init(id);
        DataContext = viewModel;
      }


    }

    private async void SaveAppButton_OnClick(object sender, EventArgs e)
    {
      await viewModel.ExecuteSaveExpenseCommand();
      NavigationService.GoBack();
    }
  }
}