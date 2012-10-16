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
			assignmentBackground.Image = Theme.AssignmentActive;

			SetActiveAssignment (false, false);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			Theme.TransitionWindow ();

			assignmentViewModel.LoadAssignmentsAsync ().ContinueOnUIThread (_ => {
				if (assignmentViewModel.ActiveAssignment == null) {
					SetActiveAssignment (false);
				} else {
					SetActiveAssignment (true);
				}

				tableView.ReloadData ();
			});
		}

		private void SetActiveAssignment (bool visible, bool animate = true)
		{
			if (visible != activeAssignmentVisible) {
				if (animate) {
					UIView.BeginAnimations ("ChangeActiveAssignment");
					UIView.SetAnimationDuration (.3);
					UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
				}

				activeAssignment.Alpha = visible ? 1 : 0;

				var frame = tableView.Frame;
				float height = 97;
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
