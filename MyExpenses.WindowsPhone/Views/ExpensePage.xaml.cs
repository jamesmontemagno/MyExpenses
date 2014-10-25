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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;
using MyExpenses.WindowsPhone.Resources;

namespace MyExpenses.WindowsPhone
{
  public partial class ExpensePage : PhoneApplicationPage
  {
    private ExpenseViewModel viewModel;
    private IMessageDialog dialog;
    // Constructor
    public ExpensePage()
    {
      InitializeComponent();
      dialog = ServiceContainer.Resolve<IMessageDialog>();
    }

    // When page is navigated to set data context to selected item in list
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (DataContext == null)
      {
        string selectedId;
       
        viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
        NavigationContext.QueryString.TryGetValue("selectedItem", out selectedId); ;
        
       
        await viewModel.Init(selectedId);
        DataContext = viewModel;

        TextBlockExpense.Text = viewModel.Title.ToLower();
      }


    }

    private async void SaveAppButton_OnClick(object sender, EventArgs e)
    {
      await viewModel.ExecuteSaveExpenseCommand();

      if (!viewModel.CanNavigate)
        return;
      NavigationService.GoBack();
    }
  }
}