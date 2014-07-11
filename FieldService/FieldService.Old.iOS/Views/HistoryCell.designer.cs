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
		}
	}
}
