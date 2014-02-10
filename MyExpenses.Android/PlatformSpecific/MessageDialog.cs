using Android.App;
using MyExpenses.Portable.Interfaces;

namespace MyExpenses.PlatformSpecific
{
  public class MessageDialog : IMessageDialog
  {
    public void SendMessage(string message, string title = null)
    {
      var builder = new AlertDialog.Builder(Application.Context);
      builder.SetMessage(message)
             .SetTitle(title ??string.Empty)
             .SetPositiveButton("OK", delegate { });
      var dialog = builder.Create();
      dialog.Show();
    }
  }
}