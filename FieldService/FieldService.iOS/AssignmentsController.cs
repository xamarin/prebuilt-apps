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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.ViewModels;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.iOS
{
	public partial class AssignmentsController : UIViewController
	{
		readonly AssignmentViewModel assignmentViewModel;
		bool activeAssignmentVisible = true;

		public AssignmentsController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = Theme.LinenPattern;
			tableView.Source = new DataSource (this);
			assignmentButton.SetBackgroundImage (Theme.AssignmentActive, UIControlState.Normal);
			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;
			priority.TextColor = UIColor.White;
			priorityBackground.Image = Theme.NumberBox;

			status.StatusChanged += (sender, e) => {
				assignmentViewModel
					.SaveAssignment (assignmentViewModel.ActiveAssignment)
					.ContinueOnUIThread (_ => ReloadAssignments ());
			};

			SetActiveAssignmentVisible (false, false);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			Theme.TransitionWindow ();

			ReloadAssignments ();
		}

		/// <summary>
		/// Reloads the entire list of assignments
		/// </summary>
		public void ReloadAssignments()
		{
			assignmentViewModel.LoadAssignmentsAsync ().ContinueOnUIThread (_ => {
				if (assignmentViewModel.ActiveAssignment == null) {
					SetActiveAssignmentVisible (false);
				} else {
					SetActiveAssignmentVisible (true);
					LoadActiveAssignment ();
				}
				
				tableView.ReloadData ();
			});
		}

		/// <summary>
		/// Reloads a single row
		/// </summary>
		public void ReloadSingleRow(NSIndexPath indexPath)
		{
			tableView.ReloadRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
		}

		partial void ActiveAssignmentSelected ()
		{
			var window = ServiceContainer.Resolve<UIWindow> ();
			window.RootViewController = ServiceContainer.Resolve<MainController> ();
		}

		/// <summary>
		/// Sets the visibility of the active assignment, with a nice animation
		/// </summary>
		private void SetActiveAssignmentVisible (bool visible, bool animate = true)
		{
			if (visible != activeAssignmentVisible) {
				if (animate) {
					UIView.BeginAnimations ("ChangeActiveAssignment");
					UIView.SetAnimationDuration (.5);
					UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
				}

				activeAssignment.Alpha = visible ? 1 : 0;

				var frame = tableView.Frame;
				float height = 95;
				if (visible) {
					frame.Y += height;
					frame.Height -= height;
				} else {
					frame.Y -= height;
					frame.Height += height;
				}
				tableView.Frame = frame;

				if (animate) {
					UIView.CommitAnimations ();
				}
				activeAssignmentVisible = visible;
			}
		}

		/// <summary>
		/// Sets the active assignment's views
		/// </summary>
		private void LoadActiveAssignment()
		{
			var assignment = assignmentViewModel.ActiveAssignment;
			priority.Text = assignment.Priority.ToString ();
			numberAndDate.Text = string.Format ("#{0} {1}", assignment.JobNumber, assignment.StartDate.Date.ToShortDateString ());
			title.Text = assignment.Title;
			startAndEnd.Text = string.Format ("Start: {0} End: {1}", assignment.StartDate.ToShortTimeString (), assignment.EndDate.ToShortTimeString ());
			contact.TopLabel.Text = assignment.ContactName;
			contact.BottomLabel.Text = assignment.ContactPhone;
			address.TopLabel.Text = assignment.Address;
			address.BottomLabel.Text = string.Format ("{0}, {1} {2}", assignment.City, assignment.State, assignment.Zip);
			status.Assignment = assignment;
		}

		partial void Settings (NSObject sender)
		{

		}

		private class DataSource : UITableViewSource
		{
			readonly AssignmentViewModel assignmentViewModel;
				
			public DataSource (AssignmentsController controller)
			{
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
			}

			public override int RowsInSection (UITableView tableView, int section)
			{
				return assignmentViewModel.Assignments == null ? 0 : assignmentViewModel.Assignments.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var assignment = assignmentViewModel.Assignments [indexPath.Row];
				var cell = tableView.DequeueReusableCell ("AssignmentCell") as AssignmentCell;
				cell.SetAssignment (assignment, indexPath);
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var window = ServiceContainer.Resolve<UIWindow> ();
				window.RootViewController = ServiceContainer.Resolve<MainController> ();
			}
		}
	}
}
