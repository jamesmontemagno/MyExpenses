using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyExpenses.Portable.DataLayer.SQLite;
using MyExpenses.Portable.DataLayer.SQLiteBase;
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

      ServiceContainer.Register<ExpensesViewModel>();
      ServiceContainer.Register<ExpenseViewModel>();
    }
  }
}