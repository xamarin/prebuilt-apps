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
using CoreGraphics;
using System.Linq;
using Foundation;
using UIKit;
using FieldService.Data;
using FieldService.ViewModels;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Table cell for an assignment
	/// </summary>
	public partial class AssignmentCell : UITableViewCell
	{
		readonly AssignmentViewModel assignmentViewModel;
		AssignmentsController controller;
		NSIndexPath indexPath;
		Assignment assignment;
		UIAlertView alertView;
		UIView statusView;

		public AssignmentCell (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			if (!Theme.IsiOS7)
				SelectedBackgroundView = new UIImageView { Image = Theme.AssignmentBlue };
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			BackgroundView = new UIImageView ();

			accept.SetTitleColor (UIColor.White, UIControlState.Normal);
			decline.SetTitleColor (UIColor.White, UIControlState.Normal);

			numberAndDate.TextColor =
				title.TextColor =
				startAndEnd.TextColor = 
				numberAndDate.HighlightedTextColor = 
				title.HighlightedTextColor =
				startAndEnd.HighlightedTextColor = Theme.LabelColor;

			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;

			status.StatusChanged += (sender, e) => SaveAssignment ();
			status.Completed += (sender, e) => {
				var menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
				menuViewModel.MenuIndex = SectionIndex.Confirmations;
				assignmentViewModel.SelectedAssignment = status.Assignment;
				controller.PerformSegue ("AssignmentDetails", controller);
			};

			if (Theme.IsiOS7) {
				priorityBackground.Image = Theme.NumberBoxHollow;
				accept.SetTitleColor (Theme.GreenColor, UIControlState.Normal);
				decline.SetTitleColor (Theme.RedColor, UIControlState.Normal);
				accept.Font =
					decline.Font = Theme.FontOfSize (16);
				startAndEnd.Font = Theme.BoldFontOfSize (10);
				startAndEnd.TextColor = UIColor.White;

				priority.TextColor = 
					priority.HighlightedTextColor = Theme.LightGrayColor;

				//Status frame
				var frame = status.Frame;
				frame.Width /= 2;
				frame.X += frame.Width;
				status.Frame = frame;

				//Priority frame
				frame = priorityBackground.Frame;
				frame.Width = frame.Height;
				priorityBackground.Frame =
					priority.Frame = frame;

				//Start/end date
				frame = startAndEnd.Frame;
				frame.X += 4;
				startAndEnd.Frame = frame;

				//Additional green rectangle on the right
				statusView = new UIView (new CGRect (Frame.Width - 8, 0, 8, Frame.Height)) {
					BackgroundColor = Theme.YellowColor,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleLeftMargin,
				};
				AddSubview (statusView);

				//Additional box for the start/end date
				frame = startAndEnd.Frame;
				frame.X -= 4;
				frame.Y += 4;
				frame.Width = 102;
				frame.Height = 16;
				var timeBox = new UIImageView (frame) {
					Image = Theme.TimeBox,
					ContentMode = UIViewContentMode.Left,
				};
				ContentView.AddSubview (timeBox);
				ContentView.BringSubviewToFront (startAndEnd);

			} else {
				priorityBackground.Image = Theme.NumberBox;
				accept.SetBackgroundImage (Theme.Accept, UIControlState.Normal);
				decline.SetBackgroundImage (Theme.Decline, UIControlState.Normal);

				priority.TextColor = 
					priority.HighlightedTextColor = UIColor.White;
			}
		}

		/// <summary>
		/// Sets up the assignment for the cell
		/// </summary>
		public void SetAssignment (AssignmentsController controller, Assignment assignment, NSIndexPath indexPath)
		{
			this.controller = controller;
			this.assignment = assignment;
			this.indexPath = indexPath;

			//Update font size on priority
			if (assignment.Priority >= 10) {
				priority.Font = Theme.FontOfSize (14);
			} else {
				priority.Font = Theme.FontOfSize (18);
			}

			//Now make any changes dependant on the assignment passed in
			((UIImageView)BackgroundView).Image = Theme.AssignmentGrey;
			priority.Text = assignment.Priority.ToString ();
			numberAndDate.Text = string.Format ("#{0} {1}", assignment.JobNumber, assignment.StartDate.Date.ToShortDateString ());
			title.Text = assignment.CompanyName;
			startAndEnd.Text = assignment.FormatStartEndDates ();
			contact.TopLabel.Text = assignment.ContactName;
			contact.BottomLabel.Text = assignment.ContactPhone;
			address.TopLabel.Text = assignment.Address;
			address.BottomLabel.Text = string.Format ("{0}, {1} {2}", assignment.City, assignment.State, assignment.Zip);

			if (assignment.Status == AssignmentStatus.New) {
				if (statusView != null)
					statusView.Hidden = true;
				status.Hidden = true;
				accept.Hidden =
					decline.Hidden = false;

				//Alpha on disabled rows
				if (Theme.IsiOS7) {
					priority.Alpha =
						priorityBackground.Alpha =
						numberAndDate.Alpha =
						title.Alpha =
						contact.Alpha = 
						address.Alpha = 0.5f;
				}

			} else {
				if (statusView != null)
					statusView.Hidden = false;
				status.Hidden = false;
				accept.Hidden =
					decline.Hidden = true;

				//Alpha on disabled rows
				if (Theme.IsiOS7) {
					priority.Alpha =
						priorityBackground.Alpha =
						numberAndDate.Alpha =
						title.Alpha =
						contact.Alpha = 
						address.Alpha = 1;
				}

				status.Assignment = assignment;
			}
		}

		/// <summary>
		/// Event when the accept button is clicked
		/// </summary>
		partial void Accept ()
		{
			if (assignmentViewModel.ActiveAssignment == null) {
				assignment.Status = AssignmentStatus.Active;
			} else {
				assignment.Status = AssignmentStatus.Hold;
			}

			SaveAssignment ();
		}

		/// <summary>
		/// Event when the decline button is clicked
		/// </summary>
		partial void Decline ()
		{
			alertView = new UIAlertView("Decline Assignment", "Are you sure?", null, "Yes", "No");
			alertView.Dismissed += (sender, e) => {
				if (e.ButtonIndex == 0) {
					assignment.Status = AssignmentStatus.Declined;

					SaveAssignment ();
				}

				alertView.Dispose ();
				alertView = null;
			};
			alertView.Show ();
		}

		/// <summary>
		/// Event when the address is clicked
		/// </summary>
		partial void Address ()
		{
			var menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
			menuViewModel.MenuIndex = SectionIndex.Maps;
			assignmentViewModel.SelectedAssignment = assignment;
			controller.PerformSegue ("AssignmentDetails", controller);
		}

		/// <summary>
		/// Saves the assignment.
		/// </summary>
		private void SaveAssignment ()
		{
			assignmentViewModel.SaveAssignmentAsync (assignment)
				.ContinueWith (_ => {
					BeginInvokeOnMainThread (() => {
						if (assignment.Status == AssignmentStatus.Active || assignment.Status == AssignmentStatus.Declined) {
							controller.ReloadAssignments ();
						} else {
							controller.ReloadSingleRow (indexPath);
						}
					});
				});
		}

		/// <summary>
		/// We override this to remove some highlighting behavior we don't want
		/// </summary>
		public override void SetHighlighted (bool highlighted, bool animated)
		{
			base.SetHighlighted (highlighted, animated);

			numberAndDate.Highlighted = 
				title.Highlighted = 
				startAndEnd.Highlighted =
				accept.Highlighted = 
				decline.Highlighted = 
				status.Highlighted = 
				contact.Highlighted =
				address.Highlighted = false;
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
