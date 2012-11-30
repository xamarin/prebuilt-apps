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
using FieldService.Data;
using FieldService.ViewModels;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Table cell for assignment items
	/// </summary>
	public partial class AssignmentItemCell : UITableViewCell
	{
		readonly ItemViewModel itemViewModel;
		readonly AssignmentsController assignmentController;
		AssignmentItem item;

		public AssignmentItemCell (IntPtr handle) : base (handle)
		{
			BackgroundView = new UIImageView { Image = Theme.Row };
			SelectedBackgroundView = new UIImageView { Image = Theme.RowPress };
			itemViewModel = new ItemViewModel();
			assignmentController = ServiceContainer.Resolve<AssignmentsController>();
		}

		/// <summary>
		/// Sets the current assignment item
		/// </summary>
		public void SetItem(AssignmentItem item)
		{
			this.item = item;
			label.TextColor = Theme.LabelColor;
			label.Text = item.Name + " " + item.Number;
			SetChecked (item.Used);
		}

		/// <summary>
		/// Sets the appropriate image for the check box
		/// </summary>
		public void SetChecked(bool isChecked) 
		{
			Accessory = isChecked ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
		}

		/// <summary>
		/// Event when the checkbox is clicked
		/// </summary>
		public void Checked ()
		{
			var tableView = Superview as UITableView;
			item.Used = !item.Used;
			tableView.UserInteractionEnabled = false;
			SetChecked (item.Used);

			itemViewModel
				.SaveAssignmentItemAsync (assignmentController.Assignment, item)
				.ContinueOnUIThread (_ => tableView.UserInteractionEnabled = true);
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
