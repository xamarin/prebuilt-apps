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
using System.Linq;
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
			BackgroundView = new UIImageView { Image = Theme.Row };
			SelectedBackgroundView = new UIImageView { Image = Theme.RowPress };
		}

		/// <summary>
		/// Sets the labor hours on this cell
		/// </summary>
		public void SetLabor(Labor labor)
		{
			type.Text = labor.TypeAsString;
			description.Text = labor.Description;
			hours.Text = labor.Hours.ToString (@"hh\:mm\:ss");

			type.TextColor =
				description.TextColor =
				hours.TextColor = Theme.LabelColor;

			type.HighlightedTextColor = 
				description.HighlightedTextColor =
				hours.HighlightedTextColor = Theme.LabelColor;
		}

		public override void WillTransitionToState (UITableViewCellState mask)
		{
			base.WillTransitionToState (mask);

			//If the "delete" button is appearing, set the alpha to 0 so it is invisible
			if ((mask & UITableViewCellState.ShowingDeleteConfirmationMask) != 0) {
				var deleteButton = Subviews.Last();
				deleteButton.Alpha = 0;
			}
		}

		public override void DidTransitionToState (UITableViewCellState mask)
		{
			base.DidTransitionToState (mask);

			//If the "delete" button is appearing, let's reposition it and use a different animation
			if ((mask & UITableViewCellState.ShowingDeleteConfirmationMask) != 0) {
				var deleteButton = Subviews.Last();

				//Position the button
				var frame = deleteButton.Frame;
				frame.X -= 45;
				deleteButton.Frame = frame;

				//Animate the button "fading" in
				UIView.BeginAnimations ("ShowDeleteButton");
				UIView.SetAnimationDuration (.3);
				UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
				deleteButton.Alpha = 1;
				UIView.CommitAnimations ();
			}
		}

		protected override void Dispose (bool disposing)
		{
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
