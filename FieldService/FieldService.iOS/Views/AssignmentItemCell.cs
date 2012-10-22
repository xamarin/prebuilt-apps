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
		readonly AssignmentDetailsController detailsController;
		AssignmentItem item;

		public AssignmentItemCell (IntPtr handle) : base (handle)
		{
			itemViewModel = ServiceContainer.Resolve<ItemViewModel>();
			detailsController = ServiceContainer.Resolve<AssignmentDetailsController>();
		}

		public void SetItem(AssignmentItem item)
		{
			this.item = item;
			label.TextColor = Theme.LabelColor;
			label.Text = item.Name + " " + item.Number;
			checkBox.SetTitleColor (Theme.CheckboxTextColor, UIControlState.Normal);
			checkBox.SetTitleColor (Theme.LabelColor, UIControlState.Highlighted);
			SetChecked (item.Used);
		}

		private void SetChecked(bool isChecked) 
		{
			if (isChecked)
				checkBox.SetImage (Theme.CheckFilled, UIControlState.Normal);
			else
				checkBox.SetImage (Theme.CheckEmpty, UIControlState.Normal);
		}

		partial void Checked ()
		{
			checkBox.Enabled = false;
			item.Used = !item.Used;
			SetChecked (item.Used);

			itemViewModel
				.SaveAssignmentItem (detailsController.Assignment, item)
				.ContinueOnUIThread (_ => checkBox.Enabled = true);
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
