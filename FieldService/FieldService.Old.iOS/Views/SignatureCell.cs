//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// The table cell for adding signatures to an assignment
	/// </summary>
	public partial class SignatureCell : UITableViewCell
	{
		ConfirmationController controller;
		UIImage image;

		public SignatureCell (IntPtr handle) : base (handle)
		{
			BackgroundView = new UIImageView { Image = Theme.Inlay };
		}

		/// <summary>
		/// Set the signature for this cell
		/// </summary>
		public void SetSignature (ConfirmationController controller, Assignment assignment, Data.Signature signature)
		{
			this.controller = controller;

			//Dispose the previous image if there was one
			if (image != null) {
				image.Dispose ();
				image = null;
			}

			if (signature == null) {
				this.signature.Hidden = true;
				this.signature.SetBackgroundImage (null, UIControlState.Normal);

				addSignature.Hidden = false;
				addSignature.SetTitleColor (UIColor.White, UIControlState.Normal);

				if (assignment.Status != AssignmentStatus.Complete && !assignment.IsHistory) {
					addSignature.Enabled = true;
					addSignature.SetBackgroundImage (Theme.ButtonDark, UIControlState.Normal);
					addSignature.SetTitle ("Add Signature", UIControlState.Normal);
				} else {
					addSignature.Enabled = false;
					addSignature.SetBackgroundImage (null, UIControlState.Normal);
					addSignature.SetTitle ("No Signature", UIControlState.Normal);
				}
			} else {
				image = signature.Image.ToUIImage ();
				this.signature.Hidden = false;
				this.signature.SetBackgroundImage (image, UIControlState.Normal);
				this.signature.Layer.CornerRadius = 7;
				this.signature.ClipsToBounds = true;
				this.signature.Enabled = !assignment.IsReadonly;

				addSignature.Hidden = true;
			}
		}

		/// <summary>
		/// Event for adding a signature
		/// </summary>
		partial void AddSignature ()
		{
			var signatureController = new SignatureController();
			signatureController.Dismissed += (sender, e) => controller.ReloadConfirmation ();
			signatureController.PresentFromRect (Frame, Superview, UIPopoverArrowDirection.Up, true);
		}

		protected override void Dispose (bool disposing)
		{
			if (image != null) {
				image.Dispose ();
				image = null;
			}

			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
