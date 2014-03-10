// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace FieldService.iOS
{
	[Register ("ConfirmationController")]
	partial class ConfirmationController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton addPhoto { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel note { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel photos { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView photoTableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel requirement { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel signature { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView signatureTableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIToolbar toolbar { get; set; }

		[Action ("AddPhoto")]
		partial void AddPhoto ();
		
		void ReleaseDesignerOutlets ()
		{
			if (addPhoto != null) {
				addPhoto.Dispose ();
				addPhoto = null;
			}

			if (note != null) {
				note.Dispose ();
				note = null;
			}

			if (requirement != null) {
				requirement.Dispose ();
				requirement = null;
			}

			if (signature != null) {
				signature.Dispose ();
				signature = null;
			}

			if (photos != null) {
				photos.Dispose ();
				photos = null;
			}

			if (photoTableView != null) {
				photoTableView.Dispose ();
				photoTableView = null;
			}

			if (signatureTableView != null) {
				signatureTableView.Dispose ();
				signatureTableView = null;
			}

			if (toolbar != null) {
				toolbar.Dispose ();
				toolbar = null;
			}
		}
	}
}
