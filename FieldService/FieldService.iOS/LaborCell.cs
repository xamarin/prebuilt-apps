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
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// Table cell for labor hours
	/// </summary>
	public partial class LaborCell : UITableViewCell
	{
		public LaborCell (IntPtr handle) : base (handle)
		{
			BackgroundView = new UIImageView { Image = Theme.Row120 };
			SelectedBackgroundView = new UIImageView { Image = Theme.Row120Press };
		}

		/// <summary>
		/// Sets the labor hours on this cell
		/// </summary>
		public void SetLabor(Labor labor)
		{
			type.Text = labor.TypeAsString;
			description.Text = labor.Description;
			hours.Text = labor.Hours.TotalHours.ToString ("0.0");

			type.HighlightedTextColor = 
				description.HighlightedTextColor =
				hours.HighlightedTextColor = Theme.LabelColor;
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
