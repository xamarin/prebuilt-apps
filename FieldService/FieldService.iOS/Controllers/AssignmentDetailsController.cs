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
using MapKit;
using UIKit;
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
		/// <summary>
		/// Occurs when status is changed on the timer
		/// </summary>
		public event EventHandler StatusChanged;

		/// <summary>
		/// Occurs when status changes to completed
		/// </summary>
		public event EventHandler Completed;

		readonly MenuViewModel menuViewModel;
		readonly AssignmentViewModel assignmentViewModel;
		readonly Lazy<UIViewController> mapController, itemsController,	laborController, expenseController, documentController, confirmationController,	historyController;
		UIViewController lastChildController;
		SummaryController summaryController;

		public AssignmentDetailsController (IntPtr handle) : base (handle)
		{
			menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			mapController = new Lazy<UIViewController>(() => new MapController());
			itemsController = new Lazy<UIViewController>(() => Storyboard.InstantiateViewController<ItemsViewController>());
			laborController = new Lazy<UIViewController>(() => Storyboard.InstantiateViewController<LaborController>());
			expenseController = new Lazy<UIViewController>(() => Storyboard.InstantiateViewController<ExpenseController>());
			documentController = new Lazy<UIViewController>(() => Storyboard.InstantiateViewController<DocumentController>());
			confirmationController = new Lazy<UIViewController>(() => Storyboard.InstantiateViewController<ConfirmationController>());
			historyController = new Lazy<UIViewController>(() => Storyboard.InstantiateViewController<HistoryController>());
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI that is required to be setup from code
			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;
			priority.TextColor = UIColor.White;
			priorityBackground.Image = Theme.NumberBox;

			numberAndDate.TextColor =
				titleLabel.TextColor =
				startAndEnd.TextColor = Theme.LabelColor;

			//Events
			status.StatusChanged += (sender, e) => SaveAssignment ();

			status.Completed += (sender, e) => 
			{
				menuViewModel.MenuIndex = SectionIndex.Confirmations;
				assignmentViewModel.SelectedAssignment = status.Assignment;

				var method = Completed;
				if (method != null)
					Completed(this, EventArgs.Empty);
			};

			//Child controller
			lastChildController =
				summaryController = ChildViewControllers[0] as SummaryController;

			if (Theme.IsiOS7) {
				assignmentBackground.Image = Theme.AssignmentGrey;
				priority.Font = Theme.FontOfSize (18);
				startAndEnd.Font = Theme.BoldFontOfSize (10);
				startAndEnd.TextColor = UIColor.White;
				accept.SetTitleColor (Theme.GreenColor, UIControlState.Normal);
				decline.SetTitleColor (Theme.RedColor, UIControlState.Normal);
				accept.Font =
					decline.Font = Theme.FontOfSize (16);

				//Status dropdown frame
				var frame = status.Frame;
				frame.Width /= 2;
				frame.X += frame.Width + 9;
				status.Frame = frame;

				//Priority frames
				frame = priorityBackground.Frame;
				frame.Width = frame.Height;
				priorityBackground.Frame =
					priority.Frame = frame;

				//Start and end dates
				frame = startAndEnd.Frame;
				frame.X += 4;
				startAndEnd.Frame = frame;

				//Additional box for the start/end date
				frame = startAndEnd.Frame;
				frame.X -= 4;
				frame.Y += 4;
				frame.Width = 102;
				frame.Height = 16;
				var timeBox = new UIImageView (frame) {
					Image = Theme.TimeBox,
					ContentMode = UIViewContentMode.Left,
				};
				assignmentBackground.AddSubview (timeBox);
				assignmentBackground.BringSubviewToFront (startAndEnd);

			} else {
				assignmentBackground.Image = Theme.AssignmentActive;
				accept.SetBackgroundImage (Theme.Accept, UIControlState.Normal);
				decline.SetBackgroundImage (Theme.Decline, UIControlState.Normal);
			}
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
			case SectionIndex.Summary:
				nextChildController = summaryController;
				break;
			case SectionIndex.Maps:
				nextChildController = mapController.Value;
				break;
			case SectionIndex.Items:
				nextChildController = itemsController.Value;
				break;
			case SectionIndex.Labor:
				nextChildController = laborController.Value;
				break;
			case SectionIndex.Expenses:
				nextChildController = expenseController.Value;
				break;
			case SectionIndex.Documents:
				nextChildController = documentController.Value;
				break;
			case SectionIndex.Confirmations:
				nextChildController = confirmationController.Value;
				break;
			case SectionIndex.History:
				nextChildController = historyController.Value;
				break;
			default:
				//We should really never get here
				return;
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
			var assignment = assignmentViewModel.SelectedAssignment;
			if (assignment != null && IsViewLoaded) {

				//Update font size on priority
				if (assignment.Priority >= 10) {
					priority.Font = Theme.FontOfSize (14);
				} else {
					priority.Font = Theme.FontOfSize (18);
				}

				var splitController = ParentViewController as SplitController;
				if (splitController != null)
					splitController.NavigationItem.Title = assignment.JobNumberFormatted + " " + assignment.CompanyName;
				priority.Text = assignment.Priority.ToString ();
				numberAndDate.Text = string.Format ("{0} {1}", assignment.JobNumberFormatted, assignment.StartDate.Date.ToShortDateString ());
				titleLabel.Text = assignment.CompanyName;
				startAndEnd.Text = assignment.FormatStartEndDates ();
				contact.TopLabel.Text = assignment.ContactName;
				contact.BottomLabel.Text = assignment.ContactPhone;
				address.TopLabel.Text = assignment.Address;
				address.BottomLabel.Text = string.Format ("{0}, {1} {2}", assignment.City, assignment.State, assignment.Zip);
				status.Assignment = assignment;
				status.Enabled = assignment.Status != AssignmentStatus.Complete && !assignment.IsHistory;

				if (assignment.Status == AssignmentStatus.New) {
					status.Hidden = true;
					decline.Hidden =
						accept.Hidden = false;
				} else {
					status.Hidden = false;
					decline.Hidden =
						accept.Hidden = true;
				}

				var confirmationController = ChildViewControllers[0] as ConfirmationController;
				if (confirmationController != null) {
					confirmationController.ReloadConfirmation ();
				}
			}
		}

		/// <summary>
		/// Event when accept button is clicked
		/// </summary>
		partial void Accept ()
		{
			if (assignmentViewModel.ActiveAssignment == null) {
				assignmentViewModel.SelectedAssignment.Status = AssignmentStatus.Active;
			} else {
				assignmentViewModel.SelectedAssignment.Status = AssignmentStatus.Hold;
			}

			SaveAssignment ();
		}

		/// <summary>
		/// Event when decline button is clicked
		/// </summary>
		partial void Decline ()
		{
			assignmentViewModel.SelectedAssignment.Status = AssignmentStatus.Declined;
			
			SaveAssignment ();
		}

		/// <summary>
		/// Event when address is clicked
		/// </summary>
		partial void Address ()
		{
			var menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
			menuViewModel.MenuIndex = SectionIndex.Maps;
		}

		/// <summary>
		/// Saves the assignment.
		/// </summary>
		private void SaveAssignment ()
		{
			assignmentViewModel
				.SaveAssignmentAsync (assignmentViewModel.SelectedAssignment)
				.ContinueWith (t => {
					BeginInvokeOnMainThread (() => {
						UpdateAssignment ();

						var method = StatusChanged;
						if (method != null) {
							method(this, EventArgs.Empty);
						}
					});
				});
		}
	}
}
