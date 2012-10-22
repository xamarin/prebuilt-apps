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
	public partial class ConfirmationController : BaseController
	{
		public ConfirmationController (IntPtr handle) : base (handle)
		{
		}

		public Photo Photo {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			View.BackgroundColor = Theme.LinenPattern;
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

			photoTableView.Source = new PhotoTableSource ();
			signatureTableView.Source = new SignatureTableSource ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			ReloadConfirmation ();
		}

		public void ReloadConfirmation ()
		{
			photoTableView.ReloadData ();
			signatureTableView.ReloadData ();
		}

		partial void AddPhoto ()
		{
			Photo = new Photo { Date = DateTime.Now };

			PerformSegue ("AddPhoto", this);
		}

		private class PhotoTableSource : UITableViewSource
		{
			const string Identifier = "PhotoCell";
			readonly PhotoViewModel photoViewModel;
			readonly ConfirmationController confirmationController;

			public PhotoTableSource ()
			{
				photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
				confirmationController = ServiceContainer.Resolve<ConfirmationController> ();
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return photoViewModel.Photos == null ? 0 : photoViewModel.Photos.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (Identifier) as PhotoCell;
				cell.SetPhoto (photoViewModel.Photos [indexPath.Row]);
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				confirmationController.Photo = photoViewModel.Photos[indexPath.Row];
				confirmationController.PerformSegue ("AddPhoto", confirmationController);
			}
		}

		private class SignatureTableSource : UITableViewSource
		{
			const string SignatureIdentifier = "SignatureCell";
			const string CompleteIdentifier = "CompleteCell";
			readonly AssignmentDetailsController detailsController;

			public SignatureTableSource ()
			{
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
					signatureCell.SetAssignment (detailsController.Assignment);
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
