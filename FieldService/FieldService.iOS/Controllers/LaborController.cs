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
		readonly LaborViewModel laborViewModel;
		readonly AssignmentViewModel assignmentViewModel;
		UILabel title;
		UIBarButtonItem titleButton, edit, addItem, space;

		public LaborController (IntPtr handle) : base (handle)
		{
			laborViewModel = ServiceContainer.Resolve<LaborViewModel>();
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
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
			
			addItem = new UIBarButtonItem ("Add Labor", UIBarButtonItemStyle.Bordered, OnAddLabor);
			addItem.SetTitleTextAttributes (textAttributes, UIControlState.Normal);
			addItem.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			tableView.Source = new TableSource (this);
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
				UpdateToolbar ();

				laborViewModel.LoadLaborHoursAsync (assignmentViewModel.SelectedAssignment)
					.ContinueWith (_ => {
						BeginInvokeOnMainThread (() => {
							UpdateToolbar ();
							tableView.ReloadData ();
						});
					});
			}
		}

		/// <summary>
		/// Refreshes toolbar items and updates text
		/// </summary>
		private void UpdateToolbar()
		{
			var assignment = assignmentViewModel.SelectedAssignment;
			if (assignment.IsReadonly) {
				toolbar.Items = new UIBarButtonItem[] { titleButton };
			} else if (laborViewModel.LaborHours == null || laborViewModel.LaborHours.Count == 0) {
				toolbar.Items = new UIBarButtonItem[] {
					titleButton,
					space,
					addItem
				};
			} else {
				toolbar.Items = new UIBarButtonItem[] {
					titleButton,
					space,
					edit,
					addItem
				};
			}
			toolbar.SetBackgroundImage (assignment.IsHistory ? Theme.OrangeBar : Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

			//Update text
			if (laborViewModel.LaborHours == null || laborViewModel.LaborHours.Count == 0) 
				title.Text = "Labor Hours";
			else
				title.Text = string.Format ("Labor Hours ({0:0.0})", laborViewModel.LaborHours.Sum (l => l.Hours.TotalHours));
		}

		private void OnAddLabor(object sender, EventArgs e)
		{
			laborViewModel.SelectedLabor = new Labor {
				Type = LaborType.Hourly,
				AssignmentId = assignmentViewModel.SelectedAssignment.Id,
			};

			var addLaborController = Storyboard.InstantiateViewController<AddLaborController>();
			addLaborController.Dismissed += (a, b) => ReloadLabor();
			PresentViewController (addLaborController, true, null);
		}

		/// <summary>
		/// Table source for labor hours
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly LaborController controller;
			readonly LaborViewModel laborViewModel;
			readonly AssignmentViewModel assignmentViewModel;
			const string Identifier = "LaborCell";

			public TableSource (LaborController controller)
			{
				this.controller = controller;
				laborViewModel = ServiceContainer.Resolve<LaborViewModel>();
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return laborViewModel.LaborHours == null ? 0 : laborViewModel.LaborHours.Count;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				laborViewModel.SelectedLabor = laborViewModel.LaborHours[indexPath.Row];

				var addLaborController = controller.Storyboard.InstantiateViewController<AddLaborController>();
				addLaborController.Dismissed += (sender, e) => controller.ReloadLabor();
				controller.PresentViewController (addLaborController, true, null);

				//Deselect the cell, a bug in Apple's UITableView requires BeginInvoke
				BeginInvokeOnMainThread (() => {
					var cell = tableView.CellAt (indexPath);
					cell.SetSelected (false, true);
				});
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				return !assignmentViewModel.SelectedAssignment.IsReadonly;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				laborViewModel
					.DeleteLaborAsync (assignmentViewModel.SelectedAssignment, laborViewModel.LaborHours[indexPath.Row])
					.ContinueWith (_ => BeginInvokeOnMainThread(controller.ReloadLabor));
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
