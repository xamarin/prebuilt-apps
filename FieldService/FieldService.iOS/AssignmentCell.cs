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
		UIImageView statusImage;
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

				var frame = status.Frame;
				status.AddSubview (statusImage = new UIImageView (new RectangleF (14, (frame.Height - 16) / 2, 16, 16)));
				status.AddSubview (new UIImageView (new RectangleF (frame.Width - 23, (frame.Height - 7) / 2, 13, 7)) { Image = Theme.Arrow });

				status.SetBackgroundImage (Theme.DropDown, UIControlState.Normal);
				accept.SetBackgroundImage (Theme.Accept, UIControlState.Normal);
				decline.SetBackgroundImage (Theme.Decline, UIControlState.Normal);
				priority.TextColor = 
					priority.HighlightedTextColor = UIColor.White;
				priorityBackground.Image = Theme.NumberBox;
				
				numberAndDate.HighlightedTextColor =
					title.HighlightedTextColor =
					startAndEnd.HighlightedTextColor = Theme.LabelColor;

				contact.IconImage = Theme.IconPhone;
				address.IconImage = Theme.Map;

				loaded = true;
			}

			//Now make any changes dependant on the assignment passed in
			((UIImageView)BackgroundView).Image = assignment.Status == AssignmentStatus.Enroute ? Theme.AssignmentBlue : Theme.AssignmentGrey;
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

				statusImage.Image = ImageForStatus (assignment.Status);
				status.SetTitle (assignment.StatusAsString, UIControlState.Normal);
			}
		}

		private UIImage ImageForStatus (AssignmentStatus status)
		{
			switch (status) {
			case AssignmentStatus.Enroute:
			case AssignmentStatus.Complete:
				return Theme.IconActive;
			case AssignmentStatus.Declined:
				return Theme.IconComplete;
			case AssignmentStatus.InProgress:
			case AssignmentStatus.Accepted:
			case AssignmentStatus.New:
				return Theme.IconHold;
			default:
				throw new InvalidOperationException ("No picture for status: " + status);
			}
		}

		partial void Accept ()
		{
			assignment.Status = AssignmentStatus.Accepted;

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
					var tableView = Superview as UITableView;
					tableView.ReloadRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				});
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
