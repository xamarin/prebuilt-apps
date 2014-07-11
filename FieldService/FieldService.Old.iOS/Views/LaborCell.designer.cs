// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("LaborCell")]
	partial class LaborCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel type { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel description { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel hours { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (type != null) {
				type.Dispose ();
				type = null;
			}

			if (description != null) {
				description.Dispose ();
				description = null;
			}

			if (hours != null) {
				hours.Dispose ();
				hours = null;
			}
		}
	}
}
