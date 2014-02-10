using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
