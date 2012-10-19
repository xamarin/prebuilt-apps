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
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for displaying the summary about an assignment
	/// </summary>
	public partial class SummaryController : BaseController
	{
		readonly AssignmentDetailsController detailsController;

		public SummaryController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			detailsController = ServiceContainer.Resolve<AssignmentDetailsController>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI that has to be setup from code
			descriptionBackground.Image = Theme.RowEnd;
			itemsBackground.Image = 
				hoursBackground.Image = 
				expensesBackground.Image = Theme.Inlay;
			itemsLabel.TextColor =
				items.TextColor =
				hoursLabel.TextColor = 
				hours.TextColor =
				expensesLabel.TextColor =
				expenses.TextColor = UIColor.White;

			//Setup our toolbar
			var label = new UILabel (new RectangleF(0, 0, 100, 36)) { 
				Text = "Description",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
			};
			var descriptionButton = new UIBarButtonItem(label);

			//Removed temporarily
//			var viewHistory = new UIBarButtonItem("View History", UIBarButtonItemStyle.Bordered, delegate {	});
//			viewHistory.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
//			viewHistory.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);

			toolbar.Items = new UIBarButtonItem[] { 
				descriptionButton, 
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), 
				//viewHistory 
			};

			//Temporarily hide expenses
			expenses.Hidden = 
				expensesLabel.Hidden =
				expensesBackground.Hidden = true;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Update for assignment
			var assignment = detailsController.Assignment;
			description.Text = assignment.Description;
			descriptionTitle.Text = assignment.Title;
			items.Text = assignment.TotalItems.ToString ();
			hours.Text = assignment.TotalHours.TotalHours.ToString ("0.0");
			expenses.Text = assignment.TotalExpenses.ToString ("$0.00");
		}
	}
}
