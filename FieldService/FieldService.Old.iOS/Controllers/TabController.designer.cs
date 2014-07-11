// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("TabController")]
	partial class TabController
	{
		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem settings { get; set; }

		[Action ("Settings:")]
		partial void Settings (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (settings != null) {
				settings.Dispose ();
				settings = null;
			}
		}
	}
}
