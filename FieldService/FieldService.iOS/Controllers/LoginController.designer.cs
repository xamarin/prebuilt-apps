// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("LoginController")]
	partial class LoginController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField username { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField password { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel companyName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton login { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView box { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView container { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIActivityIndicatorView indicator { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton questionMark { get; set; }

		[Action ("Login")]
		partial void Login ();

		[Action ("Help")]
		partial void Help ();
		
		void ReleaseDesignerOutlets ()
		{
			if (username != null) {
				username.Dispose ();
				username = null;
			}

			if (password != null) {
				password.Dispose ();
				password = null;
			}

			if (companyName != null) {
				companyName.Dispose ();
				companyName = null;
			}

			if (login != null) {
				login.Dispose ();
				login = null;
			}

			if (box != null) {
				box.Dispose ();
				box = null;
			}

			if (container != null) {
				container.Dispose ();
				container = null;
			}

			if (indicator != null) {
				indicator.Dispose ();
				indicator = null;
			}

			if (questionMark != null) {
				questionMark.Dispose ();
				questionMark = null;
			}
		}
	}
}
