// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("HistoryCell")]
	partial class HistoryCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel number { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel date { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel title { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView phoneIcon { get; set; }

		[Outlet]
		FieldService.iOS.TextButton phone { get; set; }

		[Outlet]
		FieldService.iOS.TextButton address { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel length { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel description { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (number != null) {
				number.Dispose ();
				number = null;
			}

			if (date != null) {
				date.Dispose ();
				date = null;
			}

			if (title != null) {
				title.Dispose ();
				title = null;
			}

			if (phoneIcon != null) {
				phoneIcon.Dispose ();
				phoneIcon = null;
			}

			if (phone != null) {
				phone.Dispose ();
				phone = null;
			}

			if (address != null) {
				address.Dispose ();
				address = null;
			}

			if (length != null) {
				length.Dispose ();
				length = null;
			}

			if (description != null) {
				description.Dispose ();
				description = null;
			}
		}
	}
}
