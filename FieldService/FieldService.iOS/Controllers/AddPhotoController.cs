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
using FieldService.Utilities;
using FieldService.Data;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for adding a photo
	/// </summary>
	public partial class AddPhotoController : BaseController
	{
		readonly ConfirmationController confirmationController;
		readonly AssignmentDetailsController detailsController;
		readonly PhotoViewModel photoViewModel;
		UIImage image;
		UIAlertView alertView;

		public AddPhotoController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			confirmationController = ServiceContainer.Resolve<ConfirmationController> ();
			detailsController = ServiceContainer.Resolve<AssignmentDetailsController> ();
			photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			photoFrame.Image = Theme.PhotoFrame;
			descriptionBackground.Image = Theme.ModalInlay;
			description.ShouldReturn = t => {
				Save ();
				return false;
			};
			cancel.SetTitleTextAttributes (new UITextAttributes () { TextColor = UIColor.White }, UIControlState.Normal);
			cancel.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			deleteButton.SetBackgroundImage (Theme.DeleteButton, UIControlState.Normal);
			deleteButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			
			var label = new UILabel (new RectangleF (0, 0, 80, 36)) { 
				Text = "Photo",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (18),
			};
			var labor = new UIBarButtonItem (label);

			var done = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Bordered, (sender, e) => Save ());
			done.SetTitleTextAttributes (new UITextAttributes () { TextColor = UIColor.White }, UIControlState.Normal);
			done.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			toolbar.Items = new UIBarButtonItem[] {
				cancel,
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				labor,
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				done,
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			description.ResignFirstResponder ();

			var photo = confirmationController.Photo;
			if (photo != null) {
				if (image != null)
					image.Dispose ();

				this.photo.Image = 
					image = photo.Image.ToUIImage ();
				description.Text = photo.Description;
				date.Text = photo.Date.ToShortDateString ();
				time.Text = photo.Date.ToShortTimeString ();
			}
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);

			//Free up memory when the screen is dismissed
			if (image != null) {
				image.Dispose ();
				photo.Image =
					image = null;
			}
		}

		/// <summary>
		/// Called to save the photo
		/// </summary>
		private void Save()
		{
			var photo = confirmationController.Photo;

			//only do this if it is a new photo
			if (confirmationController.IsNew)
				photo.Image = image.ToByteArray ();

			photo.Description = description.Text;

			photoViewModel
				.SavePhoto (detailsController.Assignment, photo)
				.ContinueOnUIThread (_ => {
					confirmationController.ReloadConfirmation ();
					DismissViewController (true, delegate { });
				});
		}

		/// <summary>
		/// Event when the delete photo buttton is pressed
		/// </summary>
		partial void DeletePhoto ()
		{
			alertView = new UIAlertView ("Delete?", "Are you sure?", null, "Yes", "No");
			alertView.Dismissed += (sender, e) => {

				if (e.ButtonIndex == 0) {
					photoViewModel
						.DeletePhoto (detailsController.Assignment, confirmationController.Photo)
						.ContinueOnUIThread (_ => {
							confirmationController.ReloadConfirmation ();
							DismissViewController (true, delegate { });
						});
				}

				alertView.Dispose ();
				alertView = null;
			};
			alertView.Show ();
		}

		/// <summary>
		/// Event for cancel button
		/// </summary>
		partial void Cancel (NSObject sender)
		{
			DismissViewController (true, delegate { });
		}
	}
}
