// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace FieldService.iOS
{
	[Register ("AddExpenseController")]
	partial class AddExpenseController
	{
		[Outlet]
		MonoTouch.UIKit.UIToolbar toolbar { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem cancel { get; set; }

		[Action ("Cancel:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Cancel (MonoTouch.UIKit.UIBarButtonItem sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
