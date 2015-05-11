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
using Foundation;
using UIKit;

namespace MyExpenses.iOS.Helpers
{
  public static class UIHelpers
  {
    public static NSObject Invoker;
    /// <summary>
    /// Ensures the invoked on main thread.
    /// </summary>
    /// <param name="action">Action to run on main thread.</param>
    public static void EnsureInvokedOnMainThread(Action action)
    {
      if (NSThread.Current.IsMainThread)
      {
        action();
        return;
      }
      if (Invoker == null)
        Invoker = new NSObject();

      Invoker.BeginInvokeOnMainThread(() => action());
    }
  }
}