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
	[Register ("SignatureCell")]
	partial class SignatureCell
	{
		[Outlet]
		MonoTouch.UIKit.UIButton addSignature { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton signature { get; set; }

		[Action ("AddSignature")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void AddSignature (MonoTouch.UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
