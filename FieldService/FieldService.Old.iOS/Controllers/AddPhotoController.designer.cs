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

		[Action ("Choose")]
		partial void Choose ();

		[Action ("Cancel:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Cancel (MonoTouch.UIKit.UIBarButtonItem sender);

		[Action ("DeletePhoto")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void DeletePhoto (MonoTouch.UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
