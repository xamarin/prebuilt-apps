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
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for the assignment summary page
	/// </summary>
	public partial class AssignmentDetailsController : UIViewController
	{
		public AssignmentDetailsController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);
		}

		public Assignment Assignment {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI that is required to be setup from code
			View.BackgroundColor = Theme.LinenPattern;
			assignmentBackground.Image = Theme.AssignmentActive;
			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;
			priority.TextColor = UIColor.White;
			priorityBackground.Image = Theme.NumberBox;
			descriptionBackground.Image = Theme.Row180End;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (Assignment != null) {
				priority.Text = Assignment.Priority.ToString ();
				numberAndDate.Text = string.Format ("#{0} {1}", Assignment.JobNumber, Assignment.StartDate.Date.ToShortDateString ());
				title.Text = Assignment.Title;
				startAndEnd.Text = string.Format ("Start: {0} End: {1}", Assignment.StartDate.ToShortTimeString (), Assignment.EndDate.ToShortTimeString ());
				contact.TopLabel.Text = Assignment.ContactName;
				contact.BottomLabel.Text = Assignment.ContactPhone;
				address.TopLabel.Text = Assignment.Address;
				address.BottomLabel.Text = string.Format ("{0}, {1} {2}", Assignment.City, Assignment.State, Assignment.Zip);
				description.Text = Assignment.Description;
				status.Assignment = Assignment;
			}
		}
	}
}
