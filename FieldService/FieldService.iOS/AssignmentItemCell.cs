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

namespace FieldService.iOS
{
	public partial class AssignmentItemCell : UITableViewCell
	{
		AssignmentItem item;
		bool loaded = false;

		public AssignmentItemCell (IntPtr handle) : base (handle)
		{
		}

		public void SetItem(AssignmentItem item)
		{
			//Only needs to happen once
			if (!loaded) {
				loaded = true;
			}

			this.item = item;
			label.Text = item.Name + " " + item.Number;
			SetChecked (item.Used);
		}

		private void SetChecked(bool isChecked) 
		{
			if (isChecked)
				checkBox.SetImage (Theme.CheckFilled, UIControlState.Normal);
			else
				checkBox.SetImage (Theme.CheckEmpty, UIControlState.Normal);
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
