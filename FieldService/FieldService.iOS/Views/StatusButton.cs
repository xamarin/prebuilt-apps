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
using FieldService.Utilities;

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
			AddSubview (statusImage = new UIImageView (new RectangleF (12, (frame.Height - 16) / 2, 16, 16)));
			AddSubview (new UIImageView (new RectangleF (frame.Width - 23, (frame.Height - 7) / 2, 13, 7)) { Image = Theme.Arrow });

			SetTitleColor (Theme.LabelColor, UIControlState.Normal);
			SetBackgroundImage (Theme.DropDown, UIControlState.Normal);

			TouchUpInside += OnTouchUpInside;
		}

		/// <summary>
		/// The current assignment
		/// </summary>
		public Assignment Assignment
		{
			get { return assignment; }
			set
			{
				assignment = value;
				
				//Update the button
				statusImage.Image = ImageForStatus (assignment.Status);
				SetTitle (assignment.Status.ToString (), UIControlState.Normal);
			}
		}

		/// <summary>
		/// Event when clicked
		/// </summary>
		private void OnTouchUpInside(object sender, EventArgs e)
		{
			statusSheet = new AssignmentStatusSheet ();
			statusSheet.Dismissed += OnStatusSheetDismissed;
			statusSheet.ShowFrom (Frame, Superview, true);
		}

		private void OnStatusSheetDismissed(object sender, UIButtonEventArgs e)
		{
			if (statusSheet.Status.HasValue && assignment != null && assignment.Status != statusSheet.Status) {
				if (statusSheet.Status != AssignmentStatus.Complete) {
					assignment.Status = statusSheet.Status.Value;

					var method = StatusChanged;
					if (method != null) {
						method(this, EventArgs.Empty);
					}
				} else {
					var assignmentController = ServiceContainer.Resolve<AssignmentsController>();
					assignmentController.Assignment = assignment;
					assignmentController.PerformSegue ("AssignmentDetails", assignmentController);

					var menuController = ServiceContainer.Resolve<MenuController>();
					menuController.ShowConfirmation ();
				}
			}

			statusSheet.Dismissed -= OnStatusSheetDismissed;
			statusSheet.Dispose ();
			statusSheet = null;
		}

		/// <summary>
		/// Returns appropriate image for each status
		/// </summary>
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
