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


    public Task<Expense> GetExpenseAsync(string id)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Expense>> GetExpensesAsync()
    {
      throw new NotImplementedException();
    }

    public Task SyncExpensesAsync()
    {
      throw new NotImplementedException();
    }

    public Task<Expense> SaveExpenseAsync(Expense expense)
    {
      throw new NotImplementedException();
    }

    public Task<string> DeleteExpenseAsync(Expense expense)
    {
      throw new NotImplementedException();
    }

    public string UserId
    {
      get { throw new NotImplementedException(); }
    }

    public Task Init()
    {
      throw new NotImplementedException();
    }
  }
}

