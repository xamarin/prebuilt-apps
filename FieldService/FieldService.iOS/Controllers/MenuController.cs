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
using CoreGraphics;
using Foundation;
using UIKit;
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
		/// <summary>
		/// Occurs when the menu is changed
		/// </summary>
		public event EventHandler<MenuEventArgs> MenuChanged; 

		/// <summary>
		/// Occurs when the assignment is completed via the StatusButton
		/// </summary>
		public event EventHandler AssignmentCompleted;

		/// <summary>
		/// Occurs when status is changed on the timer
		/// </summary>
		public event EventHandler StatusChanged;

		readonly MenuViewModel menuViewModel;
		readonly AssignmentViewModel assignmentViewModel;

		public MenuController (IntPtr handle) : base (handle)
		{
			menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			//Subscribe to ViewModel events
			assignmentViewModel.HoursChanged += OnHoursChanged;
			assignmentViewModel.RecordingChanged += OnRecordingChanged;
			menuViewModel.MenuIndexChanged += OnMenuIndexChanged;
		}

		private void OnMenuIndexChanged (object sender, EventArgs e)
		{
			if (IsViewLoaded) {
				ChangeSelection (menuViewModel.MenuIndex);
			}
		}

		private void OnRecordingChanged (object sender, EventArgs e)
		{
			if (IsViewLoaded) {
				record.SetImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);
			}
		}

		private void OnHoursChanged (object sender, EventArgs e)
		{
			if (IsViewLoaded) {
				timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);

			//Do this because the ViewModel hangs around for the lifetime of the app
			assignmentViewModel.HoursChanged -= OnHoursChanged;
			assignmentViewModel.RecordingChanged -= OnRecordingChanged;
			menuViewModel.MenuIndexChanged -= OnMenuIndexChanged;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI we have to setup from code
			tableView.Source = new TableSource (this);
			timerLabel.TextColor = Theme.LabelColor;

			status.StatusChanged += (sender, e) => SaveAssignment ();
			status.Completed += (sender, e) => {

				menuViewModel.MenuIndex = SectionIndex.Confirmations;
				assignmentViewModel.SelectedAssignment = status.Assignment;

				var method = AssignmentCompleted;
				if (method != null) {
					method(this, EventArgs.Empty);
				}
			};

			if (Theme.IsiOS7) {
				//NOTE: tableView.Style is readonly, so we have to do a little work to make our iOS 7 tableView not look like a grouped tableView
				tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
				tableView.BackgroundColor = Theme.LinenPattern;
				tableView.RowHeight = 50;

				timerBackground.BackgroundColor = UIColor.White;
				timerLabel.Font = Theme.FontOfSize (24);

				//Move the table around
				var frame = View.Frame;
				frame.X = 0;
				frame.Y = -2;
				frame.Height += 2;
				tableView.Frame = frame;

				//Move the timer
				frame = timerBackground.Frame;
				frame.Y += frame.Height - 80;
				frame.Height = 80;
				timerBackground.Frame = frame;

				//Move the status dropdown
				frame = status.Frame;
				frame.X += 185;
				frame.Y += 55;
				frame.Width = 90;
				status.Frame = frame;

				//Move the timer label
				frame = timerLabel.Frame;
				frame.X -= 60;
				timerLabel.Frame = frame;

				//Move the record button
				frame = record.Frame;
				frame.X = (float)Math.Floor((timerBackground.Frame.Width - frame.Width) / 2);
				record.Frame = frame;

				//Additional green rectangle on the right
				var statusView = new UIView (new CGRect (timerBackground.Frame.Width - 8, 0, 8, timerBackground.Frame.Height)) {
					BackgroundColor = Theme.GreenColor,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleLeftMargin,
				};
				timerBackground.AddSubview (statusView);

			} else {
				timerBackground.Image = Theme.TimerBackground;
				View.BackgroundColor = Theme.LeftMenuColor;
				timerLabelBackground.Image = Theme.TimerField;
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Update the UI
			UpdateAssignment ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			var selected = tableView.IndexPathForSelectedRow;
			if (selected == null || selected.Row != menuViewModel.MenuIndex) {
				ChangeSelection (menuViewModel.MenuIndex, false);
			}
		}

		private void ChangeSelection (int index, bool animated = true)
		{
			nint count = tableView.NumberOfRowsInSection (0);
			if (index < count) {
				using (var indexPath = NSIndexPath.FromRowSection (index, 0)) {
					tableView.SelectRow (indexPath, animated, UITableViewScrollPosition.Top);
				
					OnMenuChanged (new MenuEventArgs { TableView = tableView, IndexPath = indexPath, Animated = false });
				}
			}
		}

		/// <summary>
		/// Sets up the UI for the active assignment
		/// </summary>
		public void UpdateAssignment()
		{
			if (IsViewLoaded) {
				if (assignmentViewModel.SelectedAssignment.Status == AssignmentStatus.Active) {
					status.Assignment = assignmentViewModel.ActiveAssignment;
					timerView.Hidden = false;
					timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
					record.SetImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);

					//Animate the timeView on screen and set its alpha to 1
					UIView.Transition (timerView, .3, UIViewAnimationOptions.CurveEaseInOut, 
						() => {
							var frame = timerView.Frame;
							frame.Y = View.Frame.Height - frame.Height;
							timerView.Frame = frame;
							timerView.Alpha = 1;
						}, null);
				} else {
					//Animate the timeView off screen and set its alpha to 0
					UIView.Transition (timerView, .3, UIViewAnimationOptions.CurveEaseInOut, 
					        () => {
							var frame = timerView.Frame;
							frame.Y = View.Frame.Height;
							timerView.Frame = frame;
							timerView.Alpha = 0;
						},
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
				.ContinueWith (t => {
					BeginInvokeOnMainThread (() => {
						UpdateAssignment ();
						
						var method = StatusChanged;
						if (method != null) {
							method(this, EventArgs.Empty);
						}
					});
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
					.ContinueWith (t => {
						BeginInvokeOnMainThread (() => record.Enabled = true);
					});
			} else {
				assignmentViewModel.RecordAsync ().ContinueWith (_ => BeginInvokeOnMainThread (() => record.Enabled = true));
			}
		}

		private void OnMenuChanged (MenuEventArgs args)
		{
			var method = MenuChanged;
			if (method != null) {
				method (this, args);
			}
		}

		/// <summary>
		/// The table source - has static cells
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly UITableViewCell summaryCell, mapCell, itemsCell, laborCell, expensesCell, documentsCell, confirmationCell, historyCell;
			readonly List<UITableViewCell> cells = new List<UITableViewCell>();
			readonly MenuController controller;
			readonly AssignmentViewModel assignmentViewModel;

			public TableSource (MenuController controller)
			{
				this.controller = controller;
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

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
				if (!Theme.IsiOS7) {
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
				} else {
					cell.IndentationLevel = 1;
					cell.TextLabel.Font = Theme.FontOfSize (18);
					cell.TextLabel.HighlightedTextColor = UIColor.White;
					cell.BackgroundColor = Theme.LinenPattern;
					cell.BackgroundView = new UIView { BackgroundColor = Theme.LinenPattern };
					cell.SelectedBackgroundView = new UIView { BackgroundColor = Theme.DarkGrayColor };
				}

				if (!cells.Contains (cell))
					cells.Add (cell);
			}

			public override nfloat GetHeightForHeader (UITableView tableView, nint section)
			{
				//NOTE: 1 is the minimum height for a header
				return Theme.IsiOS7 ? 1 : 20;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				if (assignmentViewModel.SelectedAssignment.IsHistory) {
					SetupCell (confirmationCell, end: true);
					return cells.Count - 1;
				}

				SetupCell (confirmationCell);
				SetupCell (historyCell, end: true);
				return cells.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				return cells[indexPath.Row];
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				controller.OnMenuChanged(new MenuEventArgs { TableView = tableView, IndexPath = indexPath });

				var splitController = controller.ParentViewController as SplitController;
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
