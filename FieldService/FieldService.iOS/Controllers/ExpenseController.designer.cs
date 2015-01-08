// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace FieldService.iOS
{
	[Register ("ExpenseController")]
	partial class ExpenseController
	{
		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		UIKit.UIToolbar toolbar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (toolbar != null) {
				toolbar.Dispose ();
				toolbar = null;
			}
		}
	}
}
