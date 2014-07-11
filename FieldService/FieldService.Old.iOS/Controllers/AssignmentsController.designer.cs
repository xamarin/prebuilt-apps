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
	[Register ("AssignmentsController")]
	partial class AssignmentsController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

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
		MonoTouch.UIKit.UILabel titleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel startAndEnd { get; set; }

		[Outlet]
		FieldService.iOS.TextButton contact { get; set; }

		[Outlet]
		FieldService.iOS.TextButton address { get; set; }

		[Outlet]
		FieldService.iOS.StatusButton status { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView timerBackgroundImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel timerLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton record { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView toolbarShadow { get; set; }

		[Action ("ActiveAssignmentSelected")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ActiveAssignmentSelected (MonoTouch.UIKit.UIButton sender);

		[Action ("Address")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Address (FieldService.iOS.TextButton sender);

		[Action ("Record")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Record (MonoTouch.UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
