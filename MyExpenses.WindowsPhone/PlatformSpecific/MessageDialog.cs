using System.Windows;
using MyExpenses.Portable.Interfaces;

namespace MyExpenses.PlatformSpecific
{
  public class MessageDialog : IMessageDialog
  {
    public void SendMessage(string message, string title = null)
    {
      Deployment.Current.Dispatcher.BeginInvoke(() =>
      {
        MessageBox.Show(message, title ?? string.Empty, MessageBoxButton.OK);
      });
    }
  }
}
