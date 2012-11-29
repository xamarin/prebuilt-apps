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
		readonly AssignmentDetailsController detailController;
		PhotoAlertSheet photoSheet;

		public ConfirmationController (IntPtr handle) : base (handle)
		{
			var assignmentsController = ServiceContainer.Resolve<AssignmentsController>();

			assignmentViewModel = assignmentsController.AssignmentViewModel;
			photoViewModel = new PhotoViewModel();
			detailController = ServiceContainer.Resolve<AssignmentDetailsController>();
		}

		/// <summary>
		/// The selected photo
		/// </summary>
		public Photo Photo {
			get;
			set;
		}

		/// <summary>
		/// True if the photo is a new photo
		/// </summary>
		public bool IsNew {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			photoSheet = new PhotoAlertSheet();
			photoSheet.Callback = image => {
				Photo.Image = image;
				PerformSegue ("AddPhoto", this);
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

			photoTableView.Source = new PhotoTableSource (photoViewModel);
			signatureTableView.Source = new SignatureTableSource ();
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
				signatureTableView.ReloadData ();

				photoViewModel.LoadPhotosAsync (detailController.Assignment)
					.ContinueWith (assignmentViewModel.LoadSignatureAsync (detailController.Assignment))
					.ContinueOnUIThread (_ => photoTableView.ReloadData ());
			}
		}

		/// <summary>
		/// Event when the "add photo" button is clicked
		/// </summary>
		partial void AddPhoto ()
		{
			Photo = new Photo { AssignmentId = detailController.Assignment.Id, Date = DateTime.Now };
			IsNew = true;

			photoSheet.ShowFrom (addPhoto.Frame, addPhoto.Superview, true);
		}

		/// <summary>
		/// Table source for photos
		/// </summary>
		private class PhotoTableSource : UITableViewSource
		{
			const string Identifier = "PhotoCell";
			readonly PhotoViewModel photoViewModel;
			readonly ConfirmationController confirmationController;

			public PhotoTableSource (PhotoViewModel photoViewModel)
			{
				this.photoViewModel = photoViewModel;
				confirmationController = ServiceContainer.Resolve<ConfirmationController> ();
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
				confirmationController.Photo = photoViewModel.Photos[indexPath.Section];
				confirmationController.IsNew = false;
				confirmationController.PerformSegue ("AddPhoto", confirmationController);
			}
		}

		/// <summary>
		/// Table source for signature
		/// </summary>
		private class SignatureTableSource : UITableViewSource
		{
			const string SignatureIdentifier = "SignatureCell";
			const string CompleteIdentifier = "CompleteCell";
			readonly AssignmentViewModel assignmentViewModel;
			readonly AssignmentDetailsController detailsController;

			public SignatureTableSource ()
			{
				var assignmentsController = ServiceContainer.Resolve<AssignmentsController>();
				assignmentViewModel = assignmentsController.AssignmentViewModel;
				detailsController = ServiceContainer.Resolve<AssignmentDetailsController> ();
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
					signatureCell.SetSignature (assignmentViewModel.Signature);
				} else {
					cell = tableView.DequeueReusableCell (CompleteIdentifier);
					var completeCell = cell as CompleteCell;
					completeCell.SetAssignment (detailsController.Assignment);
				}
				return cell;
			}
		}
	}
}
