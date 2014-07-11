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
	[Register ("AssignmentDetailsController")]
	partial class AssignmentDetailsController
	{
		[Outlet]
		MonoTouch.UIKit.UIView container { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView priorityBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel priority { get; set; }

		[Outlet]
		FieldService.iOS.TextButton contact { get; set; }

		[Outlet]
		FieldService.iOS.TextButton address { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel numberAndDate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel titleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel startAndEnd { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView assignmentBackground { get; set; }

		[Outlet]
		FieldService.iOS.StatusButton status { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton accept { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton decline { get; set; }

		[Action ("Accept")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Accept (MonoTouch.UIKit.UIButton sender);

		[Action ("Address")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Address (FieldService.iOS.TextButton sender);

		[Action ("Decline")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Decline (MonoTouch.UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
