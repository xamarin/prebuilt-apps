// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("PhotoCell")]
	partial class PhotoCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView photoFrame { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView photo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel date { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel description { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (photoFrame != null) {
				photoFrame.Dispose ();
				photoFrame = null;
			}

			if (photo != null) {
				photo.Dispose ();
				photo = null;
			}

			if (date != null) {
				date.Dispose ();
				date = null;
			}

			if (description != null) {
				description.Dispose ();
				description = null;
			}
		}
	}
}
