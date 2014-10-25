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
using System.IO;
using System.Linq;
using System.Text;

using MyExpenses.PlatformSpecific;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;
using Newtonsoft.Json;
#if __IOS__
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
#elif __ANDROID__
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
#elif WINDOWS_PHONE

#endif

namespace MyExpenses.Helpers
{
  public static class ServiceRegistrar
  {
    public static void Startup()
    {
#if __ANDROID__
      CurrentPlatform.Init();
#elif __IOS__
      CurrentPlatform.Init();
      SQLitePCL.CurrentPlatform.Init();
#endif

      var expenseService = new XmlExpenseService();
      //var expenseService = new AzureExpenseService();
      //expenseService.Init().Wait();

      ServiceContainer.Register<IMessageDialog>(() => new MessageDialog());
      ServiceContainer.Register<IExpenseService>(() => expenseService);
      ServiceContainer.Register<ExpensesViewModel>();
      ServiceContainer.Register<ExpenseViewModel>();
    }
  }
}