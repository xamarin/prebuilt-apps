// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace FieldService.iOS
{
	[Register ("LoginController")]
	partial class LoginController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView box { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel companyName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView container { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView hexagons { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIActivityIndicatorView indicator { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton login { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView logo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField password { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton questionMark { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField username { get; set; }

		[Action ("Help")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Help (MonoTouch.UIKit.UIButton sender);

		[Action ("Login")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Login (MonoTouch.UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
