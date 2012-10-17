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
	public partial class AssignmentCell : UITableViewCell
	{
		readonly AssignmentViewModel assignmentViewModel;
		bool loaded = false;
		NSIndexPath indexPath;
		Assignment assignment;

		public AssignmentCell (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

			SelectedBackgroundView = new UIImageView { Image = Theme.AssignmentBlue };
		}

		public void SetAssignment (Assignment assignment, NSIndexPath indexPath)
		{
			this.assignment = assignment;
			this.indexPath = indexPath;

			//First check things that need to be setup the first time around
			if (!loaded) {
				BackgroundView = new UIImageView ();

				accept.SetBackgroundImage (Theme.Accept, UIControlState.Normal);
				decline.SetBackgroundImage (Theme.Decline, UIControlState.Normal);
				priority.TextColor = 
					priority.HighlightedTextColor = UIColor.White;
				priorityBackground.Image = Theme.NumberBox;

				contact.IconImage = Theme.IconPhone;
				address.IconImage = Theme.Map;
				status.StatusChanged += (sender, e) => SaveAssignment ();

				loaded = true;
			}

			//Now make any changes dependant on the assignment passed in
			((UIImageView)BackgroundView).Image = Theme.AssignmentGrey;
			priority.Text = assignment.Priority.ToString ();
			numberAndDate.Text = string.Format ("#{0} {1}", assignment.JobNumber, assignment.StartDate.Date.ToShortDateString ());
			title.Text = assignment.Title;
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

		partial void Accept ()
		{
			assignment.Status = AssignmentStatus.Hold;

			SaveAssignment ();
		}

		partial void Decline ()
		{
			assignment.Status = AssignmentStatus.Declined;

			SaveAssignment ();
		}

		partial void Contact ()
		{

		}

		partial void Address ()
		{

		}

		private void SaveAssignment ()
		{
			assignmentViewModel.SaveAssignment (assignment)
				.ContinueOnUIThread (t => {
					var controller = ServiceContainer.Resolve<AssignmentsController> ();
					if (assignment.Status == AssignmentStatus.Active || assignment.Status == AssignmentStatus.Declined) {
						controller.ReloadAssignments ();
					} else {
						controller.ReloadSingleRow (indexPath);
					}
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
