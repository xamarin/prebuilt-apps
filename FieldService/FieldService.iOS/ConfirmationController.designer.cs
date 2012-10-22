// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("ConfirmationController")]
	partial class ConfirmationController
	{
		[Outlet]
		MonoTouch.UIKit.UIToolbar toolbar { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView photoTableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView signatureTableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton addPhoto { get; set; }

		[Action ("AddPhoto")]
		partial void AddPhoto ();
		
		void ReleaseDesignerOutlets ()
		{
			if (toolbar != null) {
				toolbar.Dispose ();
				toolbar = null;
			}

			if (photoTableView != null) {
				photoTableView.Dispose ();
				photoTableView = null;
			}

			if (signatureTableView != null) {
				signatureTableView.Dispose ();
				signatureTableView = null;
			}

			if (addPhoto != null) {
				addPhoto.Dispose ();
				addPhoto = null;
			}
		}
	}
}
