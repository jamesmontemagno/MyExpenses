using MonoTouch.UIKit;
using MyExpenses.iOS.Helpers;
using MyExpenses.Portable.Interfaces;

namespace MyExpenses.PlatformSpecific
{
  public class MessageDialog : IMessageDialog
  {
    public void SendMessage(string message, string title = null)
    {
      UIHelpers.EnsureInvokedOnMainThread(() =>
      {
        var alertView = new UIAlertView(title ?? string.Empty, message, null, "OK", null);
        alertView.Show();
      });
    }
  }
}