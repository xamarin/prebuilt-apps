// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("AddPhotoController")]
	partial class AddPhotoController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField description { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIToolbar toolbar { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton deleteButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel time { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel date { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView photo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView photoFrame { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView descriptionBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem cancel { get; set; }

		[Action ("DeletePhoto")]
		partial void DeletePhoto ();

		[Action ("Cancel:")]
		partial void Cancel (MonoTouch.Foundation.NSObject sender);

		[Action ("Choose")]
		partial void Choose ();
		
		void ReleaseDesignerOutlets ()
		{
			if (description != null) {
				description.Dispose ();
				description = null;
			}

			if (toolbar != null) {
				toolbar.Dispose ();
				toolbar = null;
			}

			if (deleteButton != null) {
				deleteButton.Dispose ();
				deleteButton = null;
			}

			if (time != null) {
				time.Dispose ();
				time = null;
			}

			if (date != null) {
				date.Dispose ();
				date = null;
			}

			if (photo != null) {
				photo.Dispose ();
				photo = null;
			}

			if (photoFrame != null) {
				photoFrame.Dispose ();
				photoFrame = null;
			}

			if (descriptionBackground != null) {
				descriptionBackground.Dispose ();
				descriptionBackground = null;
			}

			if (cancel != null) {
				cancel.Dispose ();
				cancel = null;
			}
		}
	}
}
