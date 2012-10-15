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
		MonoTouch.UIKit.UITableViewCell summaryCell { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell mapCell { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell itemsCell { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell laborCell { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell expensesCell { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell confirmationsCell { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (summaryCell != null) {
				summaryCell.Dispose ();
				summaryCell = null;
			}

			if (mapCell != null) {
				mapCell.Dispose ();
				mapCell = null;
			}

			if (itemsCell != null) {
				itemsCell.Dispose ();
				itemsCell = null;
			}

			if (laborCell != null) {
				laborCell.Dispose ();
				laborCell = null;
			}

			if (expensesCell != null) {
				expensesCell.Dispose ();
				expensesCell = null;
			}

			if (confirmationsCell != null) {
				confirmationsCell.Dispose ();
				confirmationsCell = null;
			}
		}
	}
}
