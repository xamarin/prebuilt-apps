// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("MenuController")]
	partial class MenuController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UINavigationBar navigationBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (navigationBar != null) {
				navigationBar.Dispose ();
				navigationBar = null;
			}
		}
	}
}
