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
using FieldService.ViewModels;
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for the confirmation screen
	/// </summary>
	public partial class ConfirmationController : BaseController
	{
		readonly AssignmentViewModel assignmentViewModel;
		readonly PhotoViewModel photoViewModel;
		readonly SizeF photoSize = new SizeF(475, 410); //Used for desired size of photos
		PhotoAlertSheet photoSheet;

		public ConfirmationController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			photoViewModel = ServiceContainer.Resolve<PhotoViewModel>();

			//Update photoSize for screen scale
			var scale = UIScreen.MainScreen.Scale;
			photoSize.Width *= scale;
			photoSize.Height *= scale;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			View.BackgroundColor = Theme.BackgroundColor;
			photoSheet = new PhotoAlertSheet();
			photoSheet.DesiredSize = photoSize;
			photoSheet.Callback = image => {
				photoViewModel.SelectedPhoto.Image = image.ToByteArray ();

				var addPhotoController = Storyboard.InstantiateViewController<AddPhotoController>();
				addPhotoController.Dismissed += (sender, e) => ReloadConfirmation ();
				PresentViewController(addPhotoController, true, null);
			};
			addPhoto.SetBackgroundImage (Theme.ButtonDark, UIControlState.Normal);
			addPhoto.SetTitleColor (UIColor.White, UIControlState.Normal);

			//Setup our toolbar
			var label = new UILabel (new RectangleF (0, 0, 120, 36)) { 
				Text = "Confirmations",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
			};
			var descriptionButton = new UIBarButtonItem (label);
			toolbar.Items = new UIBarButtonItem[] { descriptionButton };

			photoTableView.Source = new PhotoTableSource (this);
			signatureTableView.Source = new SignatureTableSource (this);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			ReloadConfirmation ();
		}

		/// <summary>
		/// Loads the confirmation screen's info
		/// </summary>
		public void ReloadConfirmation ()
		{
			if (IsViewLoaded) {
				var assignment = assignmentViewModel.SelectedAssignment;
				toolbar.SetBackgroundImage (assignment.IsHistory ? Theme.OrangeBar : Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

				signatureTableView.ReloadData ();

				photoViewModel.LoadPhotosAsync (assignment)
					.ContinueWith (assignmentViewModel.LoadSignatureAsync (assignment))
					.ContinueWith (_ => BeginInvokeOnMainThread (photoTableView.ReloadData));
			}
		}

		/// <summary>
		/// Event when the "add photo" button is clicked
		/// </summary>
		partial void AddPhoto ()
		{
			photoViewModel.SelectedPhoto = new Photo { AssignmentId = assignmentViewModel.SelectedAssignment.Id, Date = DateTime.Now };

			photoSheet.ShowFrom (addPhoto.Frame, addPhoto.Superview, true);
		}

		/// <summary>
		/// Table source for photos
		/// </summary>
		private class PhotoTableSource : UITableViewSource
		{
			const string Identifier = "PhotoCell";
			readonly PhotoViewModel photoViewModel;
			readonly ConfirmationController controller;

			public PhotoTableSource (ConfirmationController controller)
			{
				this.controller = controller;
				photoViewModel = ServiceContainer.Resolve<PhotoViewModel>();
			}

			public override UIView GetViewForHeader (UITableView tableView, int section)
			{
				return new UIView { BackgroundColor = UIColor.Clear };
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return photoViewModel.Photos == null ? 0 : photoViewModel.Photos.Count;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return 1;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (Identifier) as PhotoCell;
				cell.SetPhoto (photoViewModel.Photos [indexPath.Section]);
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				photoViewModel.SelectedPhoto = photoViewModel.Photos[indexPath.Section];

				var addPhotoController = controller.Storyboard.InstantiateViewController<AddPhotoController>();
				addPhotoController.Dismissed += (sender, e) => controller.ReloadConfirmation ();
				controller.PresentViewController(addPhotoController, true, null);
			}
		}

		/// <summary>
		/// Table source for signature
		/// </summary>
		private class SignatureTableSource : UITableViewSource
		{
			const string SignatureIdentifier = "SignatureCell";
			const string CompleteIdentifier = "CompleteCell";
			readonly ConfirmationController controller;
			readonly AssignmentViewModel assignmentViewModel;

			public SignatureTableSource (ConfirmationController controller)
			{
				this.controller = controller;
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			}

			public override UIView GetViewForHeader (UITableView tableView, int section)
			{
				return new UIView { BackgroundColor = UIColor.Clear };
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return 2;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return 1;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell;
				if (indexPath.Section == 0) {
					cell = tableView.DequeueReusableCell (SignatureIdentifier);
					var signatureCell = cell as SignatureCell;
					signatureCell.SetSignature (controller, assignmentViewModel.SelectedAssignment, assignmentViewModel.Signature);
				} else {
					cell = tableView.DequeueReusableCell (CompleteIdentifier);
					var completeCell = cell as CompleteCell;
					completeCell.SetAssignment (controller, assignmentViewModel.SelectedAssignment, tableView);
				}
				return cell;
			}
		}
	}
}
