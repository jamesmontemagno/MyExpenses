using System;

using MyExpenses.Portable.BusinessLayer.Contracts;
using MyExpenses.Portable.DataLayer.SQLite;

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
    }
    public string Name { get; set; }
    public string Notes { get; set; }
    public DateTime Due { get; set; }

    public string Total { get; set; }

    public string Category { get; set; }
    public bool Billable { get; set; }

    [Ignore]
    public string DueDateLongDisplay
    {
      get { return Due.ToLocalTime().ToString("D"); }
    }


    [Ignore]
    public string TotalDisplay
    {
      get { return "$" + Total; }
    }

    [Ignore]
    public string DueDateShortDisplay
    {
      get { return Due.ToLocalTime().ToString("d"); }
    }
  }
}
