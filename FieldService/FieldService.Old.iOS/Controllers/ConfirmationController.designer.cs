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
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void AddPhoto (MonoTouch.UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
