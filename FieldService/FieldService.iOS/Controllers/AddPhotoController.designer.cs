// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace FieldService.iOS
{
	[Register ("AddPhotoController")]
	partial class AddPhotoController
	{
		[Outlet]
		UIKit.UITextField description { get; set; }

		[Outlet]
		UIKit.UIToolbar toolbar { get; set; }

		[Outlet]
		UIKit.UIButton deleteButton { get; set; }

		[Outlet]
		UIKit.UILabel time { get; set; }

		[Outlet]
		UIKit.UILabel date { get; set; }

		[Outlet]
		UIKit.UIImageView photo { get; set; }

		[Outlet]
		UIKit.UIImageView photoFrame { get; set; }

		[Outlet]
		UIKit.UIImageView descriptionBackground { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem cancel { get; set; }

		[Action ("DeletePhoto")]
		partial void DeletePhoto ();

		[Action ("Cancel:")]
		partial void Cancel (Foundation.NSObject sender);

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
