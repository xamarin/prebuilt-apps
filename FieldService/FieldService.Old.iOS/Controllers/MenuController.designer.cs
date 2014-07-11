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
	[Register ("MenuController")]
	partial class MenuController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView timerView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView timerBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel timerLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView timerLabelBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton record { get; set; }

		[Outlet]
		FieldService.iOS.StatusButton status { get; set; }

		[Action ("Record")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Record (MonoTouch.UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
