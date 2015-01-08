// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace FieldService.iOS
{
	[Register ("LoginController")]
	partial class LoginController
	{
		[Outlet]
		UIKit.UIImageView box { get; set; }

		[Outlet]
		UIKit.UILabel companyName { get; set; }

		[Outlet]
		UIKit.UIView container { get; set; }

		[Outlet]
		UIKit.UIImageView hexagons { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView indicator { get; set; }

		[Outlet]
		UIKit.UIButton login { get; set; }

		[Outlet]
		UIKit.UIImageView logo { get; set; }

		[Outlet]
		UIKit.UITextField password { get; set; }

		[Outlet]
		UIKit.UIButton questionMark { get; set; }

		[Outlet]
		UIKit.UITextField username { get; set; }

		[Action ("Help")]
		partial void Help ();

		[Action ("Login")]
		partial void Login ();
		
		void ReleaseDesignerOutlets ()
		{
			if (box != null) {
				box.Dispose ();
				box = null;
			}

			if (companyName != null) {
				companyName.Dispose ();
				companyName = null;
			}

			if (container != null) {
				container.Dispose ();
				container = null;
			}

			if (indicator != null) {
				indicator.Dispose ();
				indicator = null;
			}

			if (login != null) {
				login.Dispose ();
				login = null;
			}

			if (logo != null) {
				logo.Dispose ();
				logo = null;
			}

			if (password != null) {
				password.Dispose ();
				password = null;
			}

			if (questionMark != null) {
				questionMark.Dispose ();
				questionMark = null;
			}

			if (username != null) {
				username.Dispose ();
				username = null;
			}

			if (hexagons != null) {
				hexagons.Dispose ();
				hexagons = null;
			}
		}
	}
}
