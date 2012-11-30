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
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for the assignment summary page
	/// </summary>
	public partial class AssignmentDetailsController : BaseController
	{
		readonly AssignmentsController assignmentsController;
		SplitController splitController;
		UIViewController lastChildController;
		SummaryController summaryController;

		public AssignmentDetailsController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			assignmentsController = ServiceContainer.Resolve<AssignmentsController>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			splitController = ServiceContainer.Resolve<SplitController>();

			//UI that is required to be setup from code
			assignmentBackground.Image = Theme.AssignmentActive;
			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;
			priority.TextColor = UIColor.White;
			priorityBackground.Image = Theme.NumberBox;
			accept.SetBackgroundImage (Theme.Accept, UIControlState.Normal);
			decline.SetBackgroundImage (Theme.Decline, UIControlState.Normal);
			lastChildController =
				summaryController = ServiceContainer.Resolve <SummaryController>();

			numberAndDate.TextColor =
				titleLabel.TextColor =
				startAndEnd.TextColor = Theme.LabelColor;

			//Events
			status.StatusChanged += (sender, e) => SaveAssignment ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			UpdateAssignment ();
		}

		/// <summary>
		/// Called when a menu item in MenuController is selected
		/// </summary>
		public void SectionSelected(UITableView tableView, NSIndexPath indexPath, bool animated = true) 
		{
			UIViewController nextChildController;
			switch (indexPath.Row) {
			case 0:
				//Summary
				nextChildController = summaryController;
				break;
			case 1:
				//Map
				nextChildController = ServiceContainer.Resolve <MapController>();
				break;
			case 2:
				//Items
				nextChildController = ServiceContainer.Resolve<ItemsViewController>();
				break;
			case 3:
				//Labor Hours
				nextChildController = ServiceContainer.Resolve<LaborController>();
				break;
			case 4:
				//Expenses
				nextChildController = ServiceContainer.Resolve<ExpenseController>();
				break;
			case 5:
				//Documents
				nextChildController = ServiceContainer.Resolve<DocumentController>();
				break;
			case 6:
				//Confirmations
				nextChildController = ServiceContainer.Resolve<ConfirmationController>();
				break;
			case 7:
				//History
				nextChildController = ServiceContainer.Resolve<HistoryController>();
				break;
			default:
				return; //This means this section isn't done yet
			}

			//Return if it's the same controller
			if (lastChildController == nextChildController) 
				return;

			//This fixes issues with rotation
			nextChildController.View.Frame = lastChildController.View.Frame;

			AddChildViewController (nextChildController);

			tableView.UserInteractionEnabled = false;

			if (animated) {
				Transition (lastChildController, nextChildController, .3, UIViewAnimationOptions.TransitionCrossDissolve, delegate {
				}, delegate {
					lastChildController.RemoveFromParentViewController ();
					lastChildController = nextChildController;
					tableView.UserInteractionEnabled = true;
				});
			} else {
				Transition (lastChildController, nextChildController, 0, UIViewAnimationOptions.TransitionNone, delegate {
				}, delegate {
					lastChildController.RemoveFromParentViewController ();
					lastChildController = nextChildController;
					tableView.UserInteractionEnabled = true;
				});
			}
		}

		/// <summary>
		/// Sets up the UI for the assignment
		/// </summary>
		public void UpdateAssignment ()
		{
			var assignment = assignmentsController.Assignment;
			if (assignment != null && IsViewLoaded) {

				splitController.NavigationItem.Title = assignment.JobNumberFormatted;
				priority.Text = assignment.Priority.ToString ();
				numberAndDate.Text = string.Format ("{0} {1}", assignment.JobNumberFormatted, assignment.StartDate.Date.ToShortDateString ());
				titleLabel.Text = assignment.CompanyName;
				startAndEnd.Text = string.Format ("Start: {0} End: {1}", assignment.StartDate.ToShortTimeString (), assignment.EndDate.ToShortTimeString ());
				contact.TopLabel.Text = assignment.ContactName;
				contact.BottomLabel.Text = assignment.ContactPhone;
				address.TopLabel.Text = assignment.Address;
				address.BottomLabel.Text = string.Format ("{0}, {1} {2}", assignment.City, assignment.State, assignment.Zip);
				status.Assignment = assignment;

				if (assignment.Status == AssignmentStatus.New) {
					status.Hidden = true;
					decline.Hidden =
						accept.Hidden = false;
				} else {
					status.Hidden = false;
					decline.Hidden =
						accept.Hidden = true;
				}
			}
		}

		/// <summary>
		/// Event when accept button is clicked
		/// </summary>
		partial void Accept ()
		{
			assignmentsController.Assignment.Status = AssignmentStatus.Hold;

			SaveAssignment ();
		}

		/// <summary>
		/// Event when decline button is clicked
		/// </summary>
		partial void Decline ()
		{
			assignmentsController.Assignment.Status = AssignmentStatus.Declined;
			
			SaveAssignment ();
		}

		/// <summary>
		/// Event when address is clicked
		/// </summary>
		partial void Address ()
		{
			var menuController = ServiceContainer.Resolve<MenuController>();
			menuController.ShowMaps(true);
		}

		/// <summary>
		/// Saves the assignment.
		/// </summary>
		private void SaveAssignment ()
		{
			assignmentsController.AssignmentViewModel
				.SaveAssignmentAsync (assignmentsController.Assignment)
				.ContinueOnUIThread (t => {

					UpdateAssignment ();

					var menuController = ServiceContainer.Resolve<MenuController>();
					menuController.UpdateAssignment ();

					var confirmationController = ServiceContainer.Resolve<ConfirmationController>();
					confirmationController.ReloadConfirmation ();
				});
		}
	}
}
