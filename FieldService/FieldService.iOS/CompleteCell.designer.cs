// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("CompleteCell")]
	partial class CompleteCell
	{
		[Outlet]
		MonoTouch.UIKit.UIButton completeButton { get; set; }

		[Action ("Complete")]
		partial void Complete ();
		
		void ReleaseDesignerOutlets ()
		{
			if (completeButton != null) {
				completeButton.Dispose ();
				completeButton = null;
			}
		}
	}
}
