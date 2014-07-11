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
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// History table view cell
	/// </summary>
	public partial class HistoryCell : UITableViewCell
	{
		bool loaded = false;

		public HistoryCell (IntPtr handle) : base (handle)
		{
			BackgroundView = new UIImageView { Image = Theme.Row };
			SelectedBackgroundView = new UIImageView { Image = Theme.ShortRowPress };
		}

		/// <summary>
		/// Sets up the history record for this cell
		/// </summary>
		public void SetHistory (AssignmentHistory history, bool enabled)
		{
			//Any 1-time settings
			if (!loaded) {
				number.TextColor = 
					length.TextColor =
					description.TextColor = 
					date.TextColor = Theme.LabelColor;

				phone.IconImage = Theme.IconPhone;
				phoneIcon.Image = Theme.IconPhoneDark;
				address.IconImage = Theme.Map;	
				address.UserInteractionEnabled = false;

				loaded = true;
			}

			SelectionStyle = history.Type == AssignmentHistoryType.PhoneCall || !enabled ? UITableViewCellSelectionStyle.None : UITableViewCellSelectionStyle.Blue;
			date.Text = history.Date.ToShortDateString ();
			phone.TopLabel.Text = history.ContactName;
			phone.BottomLabel.Text = history.ContactPhone;

			if (history.Type == AssignmentHistoryType.PhoneCall) {
				number.Hidden =
					address.Hidden = true;
				length.Hidden =
					description.Hidden =
					phoneIcon.Hidden = false;

				length.Text = "Length: " + history.CallLength.ToString ();
				description.Text = history.CallDescription;
			} else {
				number.Hidden =
					address.Hidden = false;
				length.Hidden =
					description.Hidden =
					phoneIcon.Hidden = true;

				number.Text = "#" + history.JobNumber;
				address.TopLabel.Text = history.Address;
				address.BottomLabel.Text = string.Format ("{0}, {1} {2}", history.City, history.State, history.Zip);
			}
			title.Text = history.CompanyName;
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
