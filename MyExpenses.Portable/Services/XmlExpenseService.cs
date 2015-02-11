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
using System.Threading.Tasks;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using Newtonsoft.Json;
using System.Linq;
using PCLStorage;

namespace MyExpenses.Portable.Services
{
  public class XmlExpenseService : IExpenseService
  {

    private readonly IMessageDialog dialog;

    public XmlExpenseService()
    {
    }


    public static Task<T> DeserializeObjectAsync<T>(string value)
    {
      return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));
    }

    public static T DeserializeObject<T>(string value)
    {
      return JsonConvert.DeserializeObject<T>(value);
    }

    List<Expense> Expenses = new List<Expense>();

    public Task<Expense> GetExpenseAsync(string id)
    {
      return Task.Run(()=>Expenses.FirstOrDefault(s => s.Id == id));
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync()
    {
      var rootFolder = FileSystem.Current.LocalStorage;

      var folder = await rootFolder.CreateFolderAsync(Folder,
          CreationCollisionOption.OpenIfExists);

      var file = await folder.CreateFileAsync(File,
          CreationCollisionOption.OpenIfExists);

      var json = await file.ReadAllTextAsync();

      if(!string.IsNullOrWhiteSpace(json))
        Expenses = DeserializeObject<List<Expense>>(json);

      return Expenses;
    }

    public Task SyncExpensesAsync()
    {
      return Task.Run(() => { });
    }

    public async Task<Expense> SaveExpenseAsync(Expense expense)
    {
      if(string.IsNullOrWhiteSpace(expense.Id))
      {
        expense.Id = DateTime.Now.ToString();
        Expenses.Add(expense);
      }
      else
      {
        var found = Expenses.FirstOrDefault(e => e.Id == expense.Id);
        if(found != null)
          found.SyncProperties(expense);
      }
      await Save();
      return expense;
    }

    public async Task<string> DeleteExpenseAsync(Expense expense)
    {
      var id = expense.Id;
      Expenses.Remove(expense);
      await Save();
      return id;
    }

    private string Folder = "Expenses";
    private string File = "expenses.json";

    private async Task Save()
    {
      var rootFolder = FileSystem.Current.LocalStorage;

      var folder = await rootFolder.CreateFolderAsync(Folder,
          CreationCollisionOption.OpenIfExists);

      var file = await folder.CreateFileAsync(File,
          CreationCollisionOption.ReplaceExisting);

      await file.WriteAllTextAsync(JsonConvert.SerializeObject(Expenses));
    }

    public string UserId
    {
      get { return string.Empty; ; }
    }

    public Task Init()
    {
      throw new NotImplementedException();
    }
  }
}

