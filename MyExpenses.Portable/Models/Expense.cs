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

using MyExpenses.Portable.BusinessLayer.Contracts;
using Newtonsoft.Json;

namespace MyExpenses.Portable.Models
{
  public class Expense : BusinessEntityBase
  {
    public Expense()
    {
      Name = string.Empty;
      Notes = string.Empty;
      Due = DateTime.Now;
      Total = "0.00";
      Category = string.Empty;
      Billable = true;
      IsVisible = true;
      IsDirty = true;
    }

    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    public bool IsDirty { get; set; }

    public bool IsVisible { get; set; }

    public string Name { get; set; }
    public string Notes { get; set; }
    public DateTime Due { get; set; }

    public string Total { get; set; }

    public string Category { get; set; }
    public bool Billable { get; set; }

    [JsonIgnore]
    public string DueDateLongDisplay
    {
      get { return Due.ToLocalTime().ToString("D"); }
    }


    [JsonIgnore]
    public string TotalDisplay
    {
      get { return "$" + Total; }
    }

    [JsonIgnore]
    public string DueDateShortDisplay
    {
      get { return Due.ToLocalTime().ToString("d"); }
    }

    public Expense(Expense expense)
    {
      SyncProperties(expense);
    }

    public void SyncProperties(Expense expense)
    {
      this.Billable = expense.Billable;
      this.Category = expense.Category;
      this.Due = expense.Due;
      this.IsVisible = expense.IsVisible;
      this.Name = expense.Name;
      this.Notes = expense.Notes;
      this.Total = expense.Total;
      this.UserId = expense.UserId;
    }
  }
}
