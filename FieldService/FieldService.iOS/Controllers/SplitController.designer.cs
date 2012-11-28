// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	partial class SplitController
	{
		[Outlet]
		MonoTouch.UIKit.UIView masterView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView detailView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (masterView != null) {
				masterView.Dispose ();
				masterView = null;
			}

			if (detailView != null) {
				detailView.Dispose ();
				detailView = null;
			}
		}
	}
}
