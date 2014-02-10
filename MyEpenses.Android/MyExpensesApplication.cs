using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using MyExpenses.Helpers;

namespace MyExpenses.Android
{
  [Application(Theme = "@android:style/Theme.Holo.Light") ]
  public class MyExpensesApplication : Application
  {
    public MyExpensesApplication(IntPtr handle, global::Android.Runtime.JniHandleOwnership transer)
      :base(handle, transer)
    {
      
    }

    public override void OnCreate()
    {
      base.OnCreate();
      ServiceRegistrar.Startup();
    }
  }
}
