using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Foundation;
using UIKit;
using MyExpenses.Helpers;
using MyExpenses.iOS.Views;

namespace MyExpenses.iOS
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Foundation.Register("AppDelegate")]
  public partial class AppDelegate : UIApplicationDelegate
  {

    // class-level declarations
    public override UIWindow Window
    {
      get;
      set;
    }
    
    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      // create a new window instance based on the screen size
       ServiceRegistrar.Startup();

      UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
      {
        TextColor = UIColor.White
      });

      UINavigationBar.Appearance.TintColor = UIColor.White;
      UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(52,152,219);

     

      return true;
    }

  }
}