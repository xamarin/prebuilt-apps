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
	/// Status button for assignments
	/// </summary>
	public partial class StatusButton : UIButton
	{
		UIImageView statusImage;
		Assignment assignment;
		AssignmentStatusSheet statusSheet;

		public event EventHandler StatusChanged;

		public StatusButton (IntPtr handle) : base (handle)
		{
			var frame = Frame;
			AddSubview (statusImage = new UIImageView (new RectangleF (14, (frame.Height - 16) / 2, 16, 16)));
			AddSubview (new UIImageView (new RectangleF (frame.Width - 23, (frame.Height - 7) / 2, 13, 7)) { Image = Theme.Arrow });
			
			SetBackgroundImage (Theme.DropDown, UIControlState.Normal);

			TouchUpInside += OnTouchUpInside;
		}

		public Assignment Assignment
		{
			get { return assignment; }
			set
			{
				if (assignment != value)
				{
					assignment = value;

					//Update the button
					statusImage.Image = ImageForStatus (assignment.Status);
					SetTitle (assignment.Status.ToString (), UIControlState.Normal);
				}
			}
		}

		private void OnTouchUpInside(object sender, EventArgs e)
		{
			statusSheet = new AssignmentStatusSheet ();
			statusSheet.Dismissed += OnStatusSheetDismissed;
			statusSheet.ShowFrom (Frame, Superview, true);
		}

		private void OnStatusSheetDismissed(object sender, UIButtonEventArgs e)
		{
			if (e.ButtonIndex != -1 && assignment.Status != statusSheet.Status) {
				assignment.Status = statusSheet.Status;
				
				var method = StatusChanged;
				if (method != null) {
					method(this, EventArgs.Empty);
				}
			}

			statusSheet.Dismissed -= OnStatusSheetDismissed;
			statusSheet.Dispose ();
			statusSheet = null;
		}

		private UIImage ImageForStatus (AssignmentStatus status)
		{
			switch (status) {
			case AssignmentStatus.Active:
			case AssignmentStatus.Complete:
				return Theme.IconActive;
			case AssignmentStatus.Declined:
				return Theme.IconComplete;
			case AssignmentStatus.Hold:
			case AssignmentStatus.New:
				return Theme.IconHold;
			default:
				throw new InvalidOperationException ("No picture for status: " + status);
			}
		}
	}
}
