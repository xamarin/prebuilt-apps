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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.ViewModels;
using FieldService.Utilities;

namespace FieldService.iOS
{
	public partial class ItemsViewController : BaseController
	{
		readonly AssignmentDetailsController detailsController;
		readonly AssignmentViewModel assignmentViewModel;
		UILabel title;
		UIBarButtonItem addItem;

		public ItemsViewController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
			detailsController = ServiceContainer.Resolve<AssignmentDetailsController> ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI that must be setup from code
			View.BackgroundColor = Theme.LinenPattern;

			title = new UILabel (new RectangleF(0, 0, 100, 36)) { 
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
			};
			var titleButton = new UIBarButtonItem(title);

			addItem = new UIBarButtonItem("Add Item", UIBarButtonItemStyle.Bordered, delegate { });
			addItem.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);

			toolbar.Items = new UIBarButtonItem[] { titleButton, new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), addItem };
			tableView.Source = new TableSource ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			title.Text = string.Format("Items ({0})", detailsController.Assignment.TotalItems);

			assignmentViewModel
				.LoadAssignmentItems (detailsController.Assignment)
				.ContinueOnUIThread (_ => tableView.ReloadData ());
		}

		/// <summary>
		/// Table source of items
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly AssignmentViewModel assignmentViewModel;
			const string Identifier = "AssignmentItemCell";

			public TableSource ()
			{
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return assignmentViewModel.AssignmentItems == null ? 0 : assignmentViewModel.AssignmentItems.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (Identifier) as AssignmentItemCell;
				cell.SetItem (assignmentViewModel.AssignmentItems[indexPath.Row]);
				return cell;
			}
		}
	}
}
