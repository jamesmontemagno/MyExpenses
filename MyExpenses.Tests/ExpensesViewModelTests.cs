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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;
using MyExpenses.Tests.Helpers;

namespace MyExpenses.Tests
{
  [TestClass]
  public class ExpensesViewModelTests
  {
    [TestInitialize]
    public void Init()
    {
      ServiceContainer.Register<IExpenseService>(()=>new ExpenseServiceMock());  
      ServiceContainer.Register<IMessageDialog>(()=>new MessageDialogMock());
      ServiceContainer.Register<ExpenseViewModel>();
      ServiceContainer.Register<ExpensesViewModel>();
    }

    [TestMethod]
    public void TestDeleteExpense()
    {

      var expenseService = ServiceContainer.Resolve<IExpenseService>();
      var expense = new Expense();
      expenseService.SaveExpense(expense);

      var viewModel = ServiceContainer.Resolve<ExpensesViewModel>();
      var task = viewModel.ExecuteDeleteExpenseCommand(expense);

      do
      {

      } while (!task.IsCompleted);

      Assert.IsTrue(!viewModel.Expenses.Any());

    }

    [TestMethod]
    public void TestGetExpenses()
    {

      var expenseService = ServiceContainer.Resolve<IExpenseService>();
      var expense = new Expense();
      expenseService.SaveExpense(expense);

      var viewModel = ServiceContainer.Resolve<ExpensesViewModel>();
      var task = viewModel.ExecuteLoadExpensesCommand();
      do
      {

      } while (!task.IsCompleted);
      

      Assert.IsTrue(viewModel.Expenses.Count == 1);

    }
  }
}
