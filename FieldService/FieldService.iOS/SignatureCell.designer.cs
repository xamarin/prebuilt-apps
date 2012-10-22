// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("SignatureCell")]
	partial class SignatureCell
	{
		[Outlet]
		MonoTouch.UIKit.UIButton addSignature { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton signature { get; set; }

		[Action ("AddSignature")]
		partial void AddSignature ();
		
		void ReleaseDesignerOutlets ()
		{
			if (addSignature != null) {
				addSignature.Dispose ();
				addSignature = null;
			}

			if (signature != null) {
				signature.Dispose ();
				signature = null;
			}
		}
	}
}
