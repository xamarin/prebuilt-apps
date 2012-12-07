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
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// This controller holds the main menu for assignments
	/// </summary>
	public partial class MenuController : UIViewController
	{
		readonly AssignmentViewModel assignmentViewModel;
		readonly AssignmentsController assignmentController;
		readonly AssignmentDetailsController detailsController;

		public MenuController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			assignmentController = ServiceContainer.Resolve<AssignmentsController>();
			assignmentViewModel = assignmentController.AssignmentViewModel;
			detailsController = ServiceContainer.Resolve<AssignmentDetailsController>();

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
			View.BackgroundColor = Theme.LeftMenuColor;
			tableView.Source = new TableSource (this);
			timerLabel.TextColor = Theme.LabelColor;
			timerBackground.Image = Theme.TimerBackground;
			timerLabelBackground.Image = Theme.TimerField;

			status.StatusChanged += (sender, e) => SaveAssignment ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Switch to the summary tab, unless we are returning from the history section
			if (!SkipSummary) {
				using (var indexPath = NSIndexPath.FromRowSection (0, 0)) {
					tableView.SelectRow (indexPath, false, UITableViewScrollPosition.Top);
					detailsController.SectionSelected (tableView, indexPath, false);
				}
			} else {
				SkipSummary = false;
			}

			//Update the UI
			UpdateAssignment ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			var splitController = ParentViewController as SplitController;
			if (splitController != null && splitController.IsHistory) {
				SkipSummary = true;
			}
		}

		/// <summary>
		/// If set to true, skips the "auto-selection" to the summary page
		/// </summary>
		public bool SkipSummary {
			get;
			set;
		}

		/// <summary>
		/// Displays the confirmation page
		/// </summary>
		public void ShowConfirmation ()
		{
			using (var indexPath = NSIndexPath.FromRowSection (6, 0)) {
				tableView.SelectRow (indexPath, false, UITableViewScrollPosition.Top);
				detailsController.SectionSelected (tableView, indexPath, false);
			}
		}

		/// <summary>
		/// Displays the maps page
		/// </summary>
		public void ShowMaps (bool animated)
		{
			using (var indexPath = NSIndexPath.FromRowSection (1, 0)) {
				tableView.SelectRow (indexPath, animated, UITableViewScrollPosition.Top);
				detailsController.SectionSelected (tableView, indexPath, animated);
			}
		}

		/// <summary>
		/// Displays the history page
		/// </summary>
		public void ShowHistory ()
		{
			using (var indexPath = NSIndexPath.FromRowSection (7, 0)) {
				tableView.SelectRow (indexPath, true, UITableViewScrollPosition.Top);
				detailsController.SectionSelected (tableView, indexPath, false);
			}
		}

		/// <summary>
		/// Sets up the UI for the active assignment
		/// </summary>
		public void UpdateAssignment()
		{
			if (IsViewLoaded) {
				if (assignmentController.Assignment.Status == AssignmentStatus.Active) {
					status.Assignment = assignmentViewModel.ActiveAssignment;
					timerView.Hidden = false;
					timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
					record.SetBackgroundImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);

					UIView.Transition (timerView, .3, UIViewAnimationOptions.CurveEaseInOut, 
						() => timerView.Alpha = 1, 
						delegate { });
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
			assignmentViewModel
				.SaveAssignmentAsync (assignmentViewModel.ActiveAssignment)
				.ContinueOnUIThread (t => {
					UpdateAssignment ();
					detailsController.UpdateAssignment ();
				});
		}

		/// <summary>
		/// Event for when the record button is clicked
		/// </summary>
		partial void Record ()
		{
			record.Enabled = false;
			if (assignmentViewModel.Recording) {
				assignmentViewModel
					.PauseAsync ()
					.ContinueOnUIThread (t => {
						record.Enabled = true;

						var laborController = ServiceContainer.Resolve<LaborController>();
						laborController.ReloadLabor ();
					});
			} else {
				assignmentViewModel.RecordAsync ().ContinueOnUIThread (t => record.Enabled = true);
			}
		}

		/// <summary>
		/// The table source - has static cells
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly UITableViewCell summaryCell, mapCell, itemsCell, laborCell, expensesCell, documentsCell, confirmationCell, historyCell;
			readonly List<UITableViewCell> cells = new List<UITableViewCell>();
			readonly AssignmentDetailsController detailsController;
			readonly MenuController menuController;

			public TableSource (MenuController menuController)
			{
				this.menuController = menuController;
				detailsController = ServiceContainer.Resolve<AssignmentDetailsController>();

				summaryCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				summaryCell.TextLabel.Text = "Summary";
				SetupCell (summaryCell, start: true);

				mapCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				mapCell.TextLabel.Text = "Map";
				SetupCell (mapCell);

				itemsCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				itemsCell.TextLabel.Text = "Items";
				SetupCell (itemsCell);

				laborCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				laborCell.TextLabel.Text = "Labor Hours";
				SetupCell (laborCell);

				expensesCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				expensesCell.TextLabel.Text = "Expenses";
				SetupCell (expensesCell);

				documentsCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				documentsCell.TextLabel.Text = "Documents";
				SetupCell (documentsCell);

				confirmationCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				confirmationCell.TextLabel.Text = "Confirmations";
				SetupCell (confirmationCell);

				historyCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				historyCell.TextLabel.Text = "History";
				SetupCell (historyCell, end: true);
			}

			private void SetupCell (UITableViewCell cell, bool start = false, bool end = false)
			{
				cell.TextLabel.TextColor = Theme.LabelColor;
				cell.TextLabel.HighlightedTextColor = UIColor.White;
				cell.BackgroundColor = UIColor.Clear;	
				if (start) {
					cell.BackgroundView = new UIImageView { Image = Theme.LeftListTop };
					cell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListTopActive };
				} else if (end) {
					cell.BackgroundView = new UIImageView { Image = Theme.LeftListEnd };
					cell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListEndActive };
				} else {
					cell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
					cell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };
				}
				cell.ImageView.Image = Theme.TransparentDot;
				cell.ImageView.HighlightedImage = Theme.Dot;
				cells.Add (cell);
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

				var splitController = menuController.ParentViewController as SplitController;
				if (splitController != null) 
					splitController.HidePopover ();
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
