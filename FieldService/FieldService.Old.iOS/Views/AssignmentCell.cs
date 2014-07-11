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
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
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
		bool loaded = false;
		AssignmentsController controller;
		NSIndexPath indexPath;
		Assignment assignment;
		UIAlertView alertView;

		public AssignmentCell (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			SelectedBackgroundView = new UIImageView { Image = Theme.AssignmentBlue };
		}

		/// <summary>
		/// Sets up the assignment for the cell
		/// </summary>
		public void SetAssignment (AssignmentsController controller, Assignment assignment, NSIndexPath indexPath)
		{
			this.controller = controller;
			this.assignment = assignment;
			this.indexPath = indexPath;

			//First check things that need to be setup the first time around
			if (!loaded) {
				BackgroundView = new UIImageView ();

				accept.SetBackgroundImage (Theme.Accept, UIControlState.Normal);
				accept.SetTitleColor (UIColor.White, UIControlState.Normal);

				decline.SetBackgroundImage (Theme.Decline, UIControlState.Normal);
				decline.SetTitleColor (UIColor.White, UIControlState.Normal);
				priority.TextColor = 
					priority.HighlightedTextColor = UIColor.White;
				priorityBackground.Image = Theme.NumberBox;

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

				loaded = true;
			}

			//Now make any changes dependant on the assignment passed in
			((UIImageView)BackgroundView).Image = Theme.AssignmentGrey;
			priority.Text = assignment.Priority.ToString ();
			numberAndDate.Text = string.Format ("#{0} {1}", assignment.JobNumber, assignment.StartDate.Date.ToShortDateString ());
			title.Text = assignment.CompanyName;
			startAndEnd.Text = string.Format ("Start: {0} End: {1}", assignment.StartDate.ToShortTimeString (), assignment.EndDate.ToShortTimeString ());
			contact.TopLabel.Text = assignment.ContactName;
			contact.BottomLabel.Text = assignment.ContactPhone;
			address.TopLabel.Text = assignment.Address;
			address.BottomLabel.Text = string.Format ("{0}, {1} {2}", assignment.City, assignment.State, assignment.Zip);

			if (assignment.Status == AssignmentStatus.New) {
				status.Hidden = true;
				accept.Hidden =
					decline.Hidden = false;
			} else {
				status.Hidden = false;
				accept.Hidden =
					decline.Hidden = true;

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
			alertView = new UIAlertView("Decline?", "Are you sure?", null, "Yes", "No");
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
