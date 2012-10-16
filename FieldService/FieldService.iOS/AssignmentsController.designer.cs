// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("AssignmentsController")]
	partial class AssignmentsController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISegmentedControl segmentedControl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView activeAssignment { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton assignmentButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView priorityBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel priority { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel numberAndDate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel title { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel startAndEnd { get; set; }

		[Outlet]
		FieldService.iOS.TextButton contact { get; set; }

		[Outlet]
		FieldService.iOS.TextButton address { get; set; }

		[Outlet]
		FieldService.iOS.StatusButton status { get; set; }

		[Action ("ActiveAssignmentSelected")]
		partial void ActiveAssignmentSelected ();

		[Action ("Settings:")]
		partial void Settings (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (segmentedControl != null) {
				segmentedControl.Dispose ();
				segmentedControl = null;
			}

			if (activeAssignment != null) {
				activeAssignment.Dispose ();
				activeAssignment = null;
			}

			if (assignmentButton != null) {
				assignmentButton.Dispose ();
				assignmentButton = null;
			}

			if (priorityBackground != null) {
				priorityBackground.Dispose ();
				priorityBackground = null;
			}

			if (priority != null) {
				priority.Dispose ();
				priority = null;
			}

			if (numberAndDate != null) {
				numberAndDate.Dispose ();
				numberAndDate = null;
			}

			if (title != null) {
				title.Dispose ();
				title = null;
			}

			if (startAndEnd != null) {
				startAndEnd.Dispose ();
				startAndEnd = null;
			}

			if (contact != null) {
				contact.Dispose ();
				contact = null;
			}

			if (address != null) {
				address.Dispose ();
				address = null;
			}

			if (status != null) {
				status.Dispose ();
				status = null;
			}
		}
	}
}
