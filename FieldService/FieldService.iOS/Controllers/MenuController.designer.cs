// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace FieldService.iOS
{
	[Register ("MenuController")]
	partial class MenuController
	{
		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		UIKit.UIView timerView { get; set; }

		[Outlet]
		UIKit.UIImageView timerBackground { get; set; }

		[Outlet]
		UIKit.UILabel timerLabel { get; set; }

		[Outlet]
		UIKit.UIImageView timerLabelBackground { get; set; }

		[Outlet]
		UIKit.UIButton record { get; set; }

		[Outlet]
		FieldService.iOS.StatusButton status { get; set; }

		[Action ("Record")]
		partial void Record ();
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (timerView != null) {
				timerView.Dispose ();
				timerView = null;
			}

			if (timerBackground != null) {
				timerBackground.Dispose ();
				timerBackground = null;
			}

			if (timerLabel != null) {
				timerLabel.Dispose ();
				timerLabel = null;
			}

			if (timerLabelBackground != null) {
				timerLabelBackground.Dispose ();
				timerLabelBackground = null;
			}

			if (record != null) {
				record.Dispose ();
				record = null;
			}

			if (status != null) {
				status.Dispose ();
				status = null;
			}
		}
	}
}
