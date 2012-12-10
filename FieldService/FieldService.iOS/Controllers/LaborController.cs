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
//    limitations under the License.using System;
using System;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;
using FieldService.ViewModels;
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for the labor section
	/// </summary>
	public partial class LaborController : BaseController
	{
		readonly AssignmentsController assignmentController;
		UILabel title;
		UIBarButtonItem titleButton, edit, addItem, space;

		public LaborController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			LaborViewModel = new LaborViewModel();
			assignmentController = ServiceContainer.Resolve <AssignmentsController> ();
		}

		/// <summary>
		/// Gets or sets the labor entry being edited
		/// </summary>
		public Labor Labor {
			get;
			set;
		}

		public LaborViewModel LaborViewModel {
			get;
			private set;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI to setup from code
			View.BackgroundColor = Theme.BackgroundColor;
			title = new UILabel (new RectangleF (0, 0, 160, 36)) { 
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
				Text = "Labor Hours",
			};
			titleButton = new UIBarButtonItem (title);
			toolbar.Items = new UIBarButtonItem[] { titleButton };

			var textAttributes = new UITextAttributes () { TextColor = UIColor.White };
			edit = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Bordered, delegate {
				edit.Title = tableView.Editing ? "Edit" : "Done";
				tableView.SetEditing (!tableView.Editing, true);
			});
			edit.SetTitleTextAttributes (textAttributes, UIControlState.Normal);
			edit.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			space = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);
			
			addItem = new UIBarButtonItem ("Add Labor", UIBarButtonItemStyle.Bordered, (sender, e) => {
				Labor = new Labor {
					Type = LaborType.Hourly,
					AssignmentId = assignmentController.Assignment.Id,
				};
				PerformSegue ("AddLabor", this);
			});
			addItem.SetTitleTextAttributes (textAttributes, UIControlState.Normal);
			addItem.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			tableView.Source = new TableSource (LaborViewModel);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			ReloadLabor ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			//Dismiss editing on the tableView
			if (tableView.Editing) {
				edit.Title = "Edit";
				tableView.SetEditing (false, true);
			}
		}

		/// <summary>
		/// Reload the labor hours
		/// </summary>
		public void ReloadLabor ()
		{
			if (IsViewLoaded) {
				var assignment = assignmentController.Assignment;
				if (assignment.Status == AssignmentStatus.Complete || assignment.IsHistory) {
					toolbar.Items = new UIBarButtonItem[] { titleButton };
				} else {
					toolbar.Items = new UIBarButtonItem[] {
						titleButton,
						space,
						edit,
						addItem
					};
				}
				toolbar.SetBackgroundImage (assignment.IsHistory ? Theme.OrangeBar : Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

				LaborViewModel.LoadLaborHoursAsync (assignment)
					.ContinueOnUIThread (_ => {
						if (LaborViewModel.LaborHours == null || LaborViewModel.LaborHours.Count == 0) 
							title.Text = "Labor Hours";
						else
							title.Text = string.Format ("Labor Hours ({0:0.0})", LaborViewModel.LaborHours.Sum (l => l.Hours.TotalHours));
						tableView.ReloadData ();
					});
			}
		}

		/// <summary>
		/// Table source for labor hours
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly LaborViewModel laborViewModel;
			readonly LaborController laborController;
			readonly AssignmentsController assignmentController;
			const string Identifier = "LaborCell";

			public TableSource (LaborViewModel laborViewModel)
			{
				this.laborViewModel = laborViewModel;
				laborController = ServiceContainer.Resolve<LaborController> ();
				assignmentController = ServiceContainer.Resolve<AssignmentsController>();
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return laborViewModel.LaborHours == null ? 0 : laborViewModel.LaborHours.Count;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				laborController.Labor = laborViewModel.LaborHours[indexPath.Row];
				laborController.PerformSegue ("AddLabor", laborController);

				//Deselect the cell, a bug in Apple's UITableView requires BeginInvoke
				BeginInvokeOnMainThread (() => {
					var cell = tableView.CellAt (indexPath);
					cell.SetSelected (false, true);
				});
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				return assignmentController.Assignment.Status != AssignmentStatus.Complete && !assignmentController.Assignment.IsHistory;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				laborViewModel
					.DeleteLaborAsync (assignmentController.Assignment, laborViewModel.LaborHours[indexPath.Row])
					.ContinueOnUIThread (_ => laborController.ReloadLabor ());
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var labor = laborViewModel.LaborHours [indexPath.Row];
				var cell = tableView.DequeueReusableCell (Identifier) as LaborCell;
				cell.SetLabor (labor);
				return cell;
			}
		}
	}
}
