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
using MyExpenses.Portable.DataLayer.SQLite;
using MyExpenses.Portable.DataLayer.SQLiteBase;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;
using Newtonsoft.Json;

namespace MyExpenses.Helpers
{
  public static class ServiceRegistrar
  {
    public static void Startup()
    {
      SQLiteConnection connection = null;
      string dbLocation = "expensesDB.db3";

#if XAMARIN_ANDROID
      var library = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      dbLocation = Path.Combine(library, dbLocation);
      connection = new Connection(dbLocation);
      
#elif XAMARIN_IOS
      var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      var libraryPath = Path.Combine(docsPath, "../Library/");
      dbLocation = Path.Combine(libraryPath, dbLocation);
      connection = new Connection(dbLocation);
#elif WINDOWS_PHONE
      connection = new Connection(dbLocation);
#endif
      
      ServiceContainer.Register<IExpenseService>(()=>new ExpenseService(connection));
      ServiceContainer.Register<IMessageDialog>(()=>new MessageDialog());
      ServiceContainer.Register<ExpensesViewModel>();
      ServiceContainer.Register<ExpenseViewModel>();
    }
  }
}