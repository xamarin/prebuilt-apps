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
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// This controller holds the main menu for assignments
	/// </summary>
	public partial class MenuController : UIViewController
	{
		readonly AssignmentViewModel assignmentViewModel;

		public MenuController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			assignmentViewModel.HoursChanged += (sender, e) => {
				if (IsViewLoaded) {
					timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
				}
			};
			assignmentViewModel.RecordingChanged += (sender, e) => {
				if (IsViewLoaded) {
					record.SetBackgroundImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);
				}
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI we have to setup from code
			View.BackgroundColor = Theme.LinenLeft;
			tableView.Source = new TableSource ();

			var backButton = new UIBarButtonItem ("Assignments", UIBarButtonItemStyle.Bordered, (sender, e) => Theme.TransitionController <AssignmentsController>());
			backButton.SetBackgroundImage (Theme.BackButton, UIControlState.Normal, UIBarMetrics.Default);
			backButton.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
			navigationBar.TopItem.LeftBarButtonItem = backButton;

			timerBackground.Image = Theme.TimerBackground;
			timerLabelBackground.Image = Theme.TimerField;

			status.StatusChanged += (sender, e) => SaveAssignment ();

			tableView.SelectRow (NSIndexPath.FromRowSection (0, 0), false, UITableViewScrollPosition.Top);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			UpdateAssignment ();
		}

		/// <summary>
		/// Sets up the UI for the active assignment
		/// </summary>
		public void UpdateAssignment()
		{
			if (IsViewLoaded) {
				if (assignmentViewModel.ActiveAssignment != null && assignmentViewModel.ActiveAssignment.Status == FieldService.Data.AssignmentStatus.Active) {
					status.Assignment = assignmentViewModel.ActiveAssignment;
					timerView.Alpha = 1;
					timerView.Hidden = false;
					timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
					record.SetBackgroundImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);
				} else {
					UIView.Transition (timerView, .3, UIViewAnimationOptions.CurveEaseInOut, 
						() => timerView.Alpha = 0, 
						() => timerView.Hidden = true);
				}
			}
		}

		/// <summary>
		/// Saves the assignment.
		/// </summary>
		private void SaveAssignment ()
		{
			assignmentViewModel.SaveAssignment (assignmentViewModel.ActiveAssignment).ContinueOnUIThread (t => UpdateAssignment ());
		}

		/// <summary>
		/// Event for when the record button is clicked
		/// </summary>
		partial void Record ()
		{
			record.Enabled = false;
			if (assignmentViewModel.Recording) {
				assignmentViewModel.Pause ().ContinueOnUIThread (t => record.Enabled = true);
			} else {
				assignmentViewModel.Record ().ContinueOnUIThread (t => record.Enabled = true);
			}
		}

		/// <summary>
		/// The table source - has static cells
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly UITableViewCell summaryCell, mapCell, itemsCell, laborCell, expensesCell, confirmationCell;
			readonly List<UITableViewCell> cells = new List<UITableViewCell>();
			readonly AssignmentDetailsController detailsController;

			public TableSource ()
			{
				detailsController = ServiceContainer.Resolve<AssignmentDetailsController>();

				summaryCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				summaryCell.TextLabel.Text = "Summary";
				summaryCell.TextLabel.TextColor = UIColor.White;
				summaryCell.BackgroundColor = UIColor.Clear;	
				summaryCell.BackgroundView = new UIImageView { Image = Theme.LeftListTop };
				summaryCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListTopActive };
				cells.Add (summaryCell);

				mapCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				mapCell.TextLabel.Text = "Map";
				mapCell.TextLabel.TextColor = UIColor.White;
				mapCell.BackgroundColor = UIColor.Clear;	
				mapCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
				mapCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };
				cells.Add (mapCell);

				itemsCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				itemsCell.TextLabel.Text = "Items";
				itemsCell.TextLabel.TextColor = UIColor.White;
				itemsCell.BackgroundColor = UIColor.Clear;	
				itemsCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
				itemsCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };
				cells.Add (itemsCell);

				laborCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				laborCell.TextLabel.Text = "Labor Hours";
				laborCell.TextLabel.TextColor = UIColor.White;
				laborCell.BackgroundColor = UIColor.Clear;	
				laborCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
				laborCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };
				cells.Add (laborCell);

				//Removed temporarily
//				expensesCell = new UITableViewCell (UITableViewCellStyle.Default, null);
//				expensesCell.TextLabel.Text = "Expenses";
//				expensesCell.TextLabel.TextColor = UIColor.White;
//				expensesCell.BackgroundColor = UIColor.Clear;
//				expensesCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
//				expensesCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };
//				cells.Add (expensesCell);

				confirmationCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				confirmationCell.TextLabel.Text = "Confirmations";
				confirmationCell.TextLabel.TextColor = UIColor.White;
				confirmationCell.BackgroundColor = UIColor.Clear;	
				confirmationCell.BackgroundView = new UIImageView { Image = Theme.LeftListEnd };
				confirmationCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListEndActive };
				cells.Add (confirmationCell);
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return cells.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				return cells[indexPath.Row];
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				detailsController.SectionSelected (tableView, indexPath);
			}

			protected override void Dispose (bool disposing)
			{
				foreach (var cell in cells) {
					cell.Dispose();
				}
				cells.Clear ();

				base.Dispose (disposing);
			}
		}
	}
}
