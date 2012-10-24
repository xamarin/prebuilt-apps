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
		readonly AssignmentDetailsController detailsController;
		UILabel title;
		UIBarButtonItem edit, addItem;

		public LaborController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			laborViewModel = ServiceContainer.Resolve <LaborViewModel> ();
			detailsController = ServiceContainer.Resolve <AssignmentDetailsController> ();
		}

		/// <summary>
		/// Gets or sets the labor entry being edited
		/// </summary>
		public Labor Labor {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI to setup from code
			View.BackgroundColor = Theme.LinenPattern;

			title = new UILabel (new RectangleF (0, 0, 160, 36)) { 
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
			};
			var titleButton = new UIBarButtonItem (title);
			
			edit = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Bordered, delegate {
				edit.Title = tableView.Editing ? "Edit" : "Done";
				tableView.SetEditing (!tableView.Editing, true);
			});
			edit.SetTitleTextAttributes (new UITextAttributes () { TextColor = UIColor.White }, UIControlState.Normal);
			edit.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			addItem = new UIBarButtonItem ("Add Labor", UIBarButtonItemStyle.Bordered, (sender, e) => {
				Labor = new Labor {
					Type = LaborType.Hourly,
					Assignment = detailsController.Assignment.ID,
				};
				PerformSegue ("AddLabor", this);
			});
			addItem.SetTitleTextAttributes (new UITextAttributes () { TextColor = UIColor.White }, UIControlState.Normal);
			addItem.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			toolbar.Items = new UIBarButtonItem[] {
				titleButton,
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				edit,
				addItem
			};

			tableView.Source = new TableSource ();
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
				laborViewModel.LoadLaborHours (detailsController.Assignment)
					.ContinueOnUIThread (_ => {
					if (laborViewModel.LaborHours == null || laborViewModel.LaborHours.Count == 0) 
						title.Text = "Labor Hours";
					else
						title.Text = string.Format ("Labor Hours ({0:0.0})", laborViewModel.LaborHours.Sum (l => l.Hours.TotalHours));
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
			readonly AssignmentDetailsController detailsController;
			const string Identifier = "LaborCell";

			public TableSource ()
			{
				laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
				laborController = ServiceContainer.Resolve<LaborController> ();
				detailsController = ServiceContainer.Resolve<AssignmentDetailsController>();
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

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				laborViewModel
					.DeleteLabor(detailsController.Assignment, laborViewModel.LaborHours[indexPath.Row])
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
