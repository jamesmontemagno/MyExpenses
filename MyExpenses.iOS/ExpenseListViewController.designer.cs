// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyExpenses.iOS
{
	[Register ("ExpenseListViewController")]
	partial class ExpenseListViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonAdd { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView ExpenseTableView { get; set; }

		[Action ("ButtonAdd_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonAdd_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonAdd != null) {
				ButtonAdd.Dispose ();
				ButtonAdd = null;
			}
			if (ExpenseTableView != null) {
				ExpenseTableView.Dispose ();
				ExpenseTableView = null;
			}
		}
	}
}
