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
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Data;

namespace FieldService.iOS
{
	public partial class ExpenseCell : UITableViewCell
	{
		public ExpenseCell (IntPtr handle) : base (handle)
		{
			BackgroundView = new UIImageView { Image = Theme.Row };
			SelectedBackgroundView = new UIImageView { Image = Theme.RowPress };
		}

		/// <summary>
		/// Sets the labor hours on this cell
		/// </summary>
		public void SetExpense (Expense expense)
		{
			type.Text = expense.Category.ToString ();
			description.Text = expense.Description;
			cost.Text = expense.Cost.ToString ("$0.00");
			if (!expense.HasPhoto) {
				icon.Hidden = true;
			} else {
				icon.Hidden = false;
				icon.Image = Theme.Camera;
			}

			type.TextColor =
				description.TextColor =
				cost.TextColor = Theme.LabelColor;

			type.HighlightedTextColor = 
				description.HighlightedTextColor =
				cost.HighlightedTextColor = Theme.LabelColor;
		}

		public override void WillTransitionToState (UITableViewCellState mask)
		{
			base.WillTransitionToState (mask);

			//If the "delete" button is appearing, set the alpha to 0 so it is invisible
			if ((mask & UITableViewCellState.ShowingDeleteConfirmationMask) != 0) {
				var deleteButton = Subviews.Last();
				deleteButton.Alpha = 
					icon.Alpha = 0;
			}
		}

		public override void DidTransitionToState (UITableViewCellState mask)
		{
			base.DidTransitionToState (mask);

			//If the "delete" button is appearing, let's reposition it and use a different animation
			if ((mask & UITableViewCellState.ShowingDeleteConfirmationMask) != 0) {
				var deleteButton = Subviews.Last ();

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
			} else {
				//Animate the icon "fading" in
				UIView.BeginAnimations ("ShowIcon");
				UIView.SetAnimationDuration (.3);
				UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
				icon.Alpha = 1;
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
