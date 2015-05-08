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

using CoreGraphics;
using Foundation;
using UIKit;

using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for displaying the summary about an assignment
	/// </summary>
	public partial class SummaryController : BaseController
	{
		readonly AssignmentViewModel assignmentViewModel;
		UIBarButtonItem viewHistory, descriptionButton;

		public SummaryController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI that has to be setup from code
			View.BackgroundColor = Theme.BackgroundColor;
			description.TextColor =
				descriptionTitle.TextColor = Theme.LabelColor;

			//Setup our toolbar
			var label = new UILabel (new CGRect (0f, 0f, 100f, 36f)) {
				Text = "Description",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16f),
			};

			descriptionButton = new UIBarButtonItem(label);

			viewHistory = new UIBarButtonItem("View History", UIBarButtonItemStyle.Bordered, (sender, e) => {
				var menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
				menuViewModel.MenuIndex = SectionIndex.History;
			});

			viewHistory.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
			viewHistory.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			if (!Theme.IsiOS7) {
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
				return;
			}

			descriptionBackground.BackgroundColor = UIColor.White;

			itemsLabel.Font =
				items.Font =
				hoursLabel.Font =
				hours.Font =
				expensesLabel.Font =
				expenses.Font = Theme.FontOfSize (18f);

			itemsLabel.TextColor =
				items.TextColor =
				hoursLabel.TextColor =
				hours.TextColor =
				expensesLabel.TextColor =
				expenses.TextColor = Theme.LabelColor;

			itemsBackground.BackgroundColor =
				hoursBackground.BackgroundColor =
				expensesBackground.BackgroundColor = Theme.LightGrayColor;

			//Move backgrounds around to appear as lines
			var frame = itemsBackground.Frame;
			frame.X = 0f;
			frame.Y += frame.Height - 1f;
			frame.Height = 1f;
			frame.Width = View.Frame.Width;
			itemsBackground.Frame = frame;

			frame = hoursBackground.Frame;
			frame.X = 0f;
			frame.Y += frame.Height - 1f;
			frame.Height = 1f;
			frame.Width = View.Frame.Width;
			hoursBackground.Frame = frame;

			frame = expensesBackground.Frame;
			frame.X = 0f;
			frame.Y += frame.Height - 1f;
			frame.Height = 1f;
			frame.Width = View.Frame.Width;
			expensesBackground.Frame = frame;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Update for assignment
			var assignment = assignmentViewModel.SelectedAssignment;
			description.Text = assignment.Description;
			descriptionTitle.Text = assignment.CompanyName;
			items.Text = assignment.TotalItems.ToString ();
			hours.Text = assignment.TotalHours.TotalHours.ToString ("0.0");
			expenses.Text = assignment.TotalExpenses.ToString ("$0.00");
			toolbar.SetBackgroundImage (assignment.IsHistory ? Theme.OrangeBar : Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

			if (!assignment.IsHistory) {
				toolbar.Items = new UIBarButtonItem[] { 
					descriptionButton, 
					new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), 
					// TODO: phone call functiality should be implemented first
					// now user can't make calls
//					viewHistory
				};
			} else {
				toolbar.Items = new UIBarButtonItem[] { 
					descriptionButton, 
				};
			}
		}
	}
}
