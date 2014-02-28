My Expenses
==========

My Expenses Cross Platform Demo - VSToolbox

Videos are available on Channel 9:

[Part 1: Cross Platform Mobile Development with Xamarin](http://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Cross-Platform-Development-With-Xamarin)

[Part 2: Using Portable Class Libraries with Xamarin](http://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Using-Portable-Class-Libraries-with-Xamarin)

Expense taking cross platform application for Windows Phone, Android, and iOS built with Xamarin inside of Visual Studio 2013. Expenses are stored locally in a Sqlite-net database. You can add new expenses and edit or delete existing. All business logic is shared in one portable class library.

Written in C# with ([Xamarin](http://www.xamarin.com))  **Created in Visual Studio 2013**

Open Source Project by ([@JamesMontemagno](http://www.twitter.com/jamesmontemagno)) 

For Windows Phone you must install SQLite for Windows Phone Extension: http://visualstudiogallery.msdn.microsoft.com/cd120b42-30f4-446e-8287-45387a4f40b7


## How much code is shared?
I have included an "Analysis Project", which will count the shared lines of code. 90% on iOS, 84% on Windows Phone, and 81% of code is shared the code is shared between projects. All of the Models, Services, View Models, and tons of helper classes are all found in one single PCL library. 

## What technology is used?
Everything is written in C# with Xamarin with a base PCL library. This project couldn't have been done without the following:

### Json.NET
https://components.xamarin.com/view/json.net - I use both the NuGet in the PCL and component for iOS for facade linking. One of the most wonderful Json libraries that I simply love. It is used to deserialize all information coming from the meetup.com APIs.

### HTTP Client Libraries
https://www.nuget.org/packages/Microsoft.Net.Http - Brings HTTP Client functionality to Windows Phone in PCL.

### Windows Phone Toolkit
http://phone.codeplex.com/ - Everyone's favorite WP toolkit!

### ANDHud
https://components.xamarin.com/view/AndHUD - Brings in a nice spinner for Xamarin.Android

### BTProgressHud
https://components.xamarin.com/view/btprogresshud - Great spinner for iOS

### MonoTouch.Dialog
http://docs.xamarin.com/guides/ios/user_interface/monotouch.dialog/ - A wonderful library for Xamarin.iOS to create user interfaces quick with not a lot of code.

### Sqlite-net PCL
https://github.com/praeclarum/sqlite-net - A wonderful library for cross platform sqlite databases.
Now in PCL form with: https://github.com/oysteinkrog/Sqlite.net-pcl


## License

  Copyright 2014  Xamarin Inc.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
   limitations under the License.
