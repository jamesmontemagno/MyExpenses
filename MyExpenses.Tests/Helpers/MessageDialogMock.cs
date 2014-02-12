using MyExpenses.Portable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpenses.Tests.Helpers
{
  public class MessageDialogMock : IMessageDialog
  {
    public void SendMessage(string message, string title = null)
    {
 
    }
  }
}
