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
using SignaturePad;
using FieldService.Data;
using FieldService.ViewModels;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for displaying the popover for signatures
	/// </summary>
	public class SignatureController : UIPopoverController
	{
		/// <summary>
		/// Occurs when dismissed.
		/// </summary>
		public event EventHandler Dismissed;

		public SignatureController ()
			: base(new UINavigationController())
		{
			var navController = ContentViewController as UINavigationController;
			navController.ViewControllers = new UIViewController[] { new ContentController(this) };
			PopoverContentSize = new SizeF(665, 400);
		}

		public override void Dismiss (bool animated)
		{
			base.Dismiss (animated);

			var method = Dismissed;
			if (method != null) {
				method(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Internal controller for setting up the SignatureView
		/// </summary>
		private class ContentController : UIViewController
		{
			readonly SignatureController controller;
			readonly AssignmentViewModel assignmentViewModel;
			SignaturePadView signatureView;
			UIBarButtonItem cancel;
			UIBarButtonItem save;

			public ContentController (SignatureController controller)
			{
				this.controller = controller;
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

				Title = "Add Signature";
			}

			public override void ViewDidLoad ()
			{
				base.ViewDidLoad ();

				cancel = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, (sender, e) => {
					controller.Dismiss (true);
				});
				cancel.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
				cancel.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

				save = new UIBarButtonItem("Save", UIBarButtonItemStyle.Bordered, (sender, e) => {

					//If blank, return
					if (signatureView.IsBlank) {
						new UIAlertView(string.Empty, "No signature!", null, "Ok").Show ();
						return;
					}

					if (assignmentViewModel.Signature == null) {
						assignmentViewModel.Signature = new Data.Signature { AssignmentId = assignmentViewModel.SelectedAssignment.Id };
					}
					assignmentViewModel.Signature.Image = signatureView.GetImage ().ToByteArray ();

					assignmentViewModel.SaveSignatureAsync ()
						.ContinueWith (_ => {
							BeginInvokeOnMainThread (() => controller.Dismiss (true));
						});
				});

				save.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
				save.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

				NavigationItem.LeftBarButtonItem = cancel;
				NavigationItem.RightBarButtonItem = save;
				NavigationController.NavigationBar.SetBackgroundImage (null, UIBarMetrics.Default);

				signatureView = new SignaturePadView(View.Frame)
				{
					AutoresizingMask = UIViewAutoresizing.All,
				};
				View.AddSubview (signatureView);
			}

			public override void ViewWillAppear (bool animated)
			{
				base.ViewWillAppear (animated);

				//Clear the signature prior to appearing
				signatureView.Clear ();
			}
		}
	}
}

