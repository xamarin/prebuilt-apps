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
using FieldService.ViewModels;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// The table cell to complete an assignment
	/// </summary>
	public partial class CompleteCell : UITableViewCell
	{
		readonly AssignmentViewModel assignmentViewModel;
		ConfirmationController controller;
		Assignment assignment;
		UITableView tableView;
		UIAlertView alertView;

		public CompleteCell (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			BackgroundView = new UIImageView { Image = Theme.Inlay };
		}

		/// <summary>
		/// Sets the assignment for this cell
		/// </summary>
		public void SetAssignment(ConfirmationController controller, Assignment assignment, UITableView tableView)
		{
			this.controller = controller;
			this.assignment = assignment;
			this.tableView = tableView;

			completeButton.Enabled = assignment.CanComplete;
			completeButton.SetBackgroundImage (Theme.Complete, UIControlState.Normal);
			completeButton.SetBackgroundImage (Theme.CompleteInactive, UIControlState.Disabled);
			completeButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			completeButton.SetTitle ("Complete", UIControlState.Normal);
			completeButton.SetTitle ("Completed", UIControlState.Disabled);
		}

		/// <summary>
		/// Event when complete is pressed
		/// </summary>
		partial void Complete ()
		{
			//Check if they signed
			if (assignmentViewModel.Signature == null) {
				new UIAlertView(string.Empty, "Signature is required.", null, "Ok").Show ();
				return;
			}

			alertView = new UIAlertView("Complete?", "Are you sure?", null, "Yes", "No");
			alertView.Dismissed += (sender, e) => {
				alertView.Dispose ();
				alertView = null;

				if (e.ButtonIndex == 0) {
					completeButton.Enabled = false;
					assignment.Status = AssignmentStatus.Complete;
					assignmentViewModel
						.SaveAssignmentAsync (assignment)
						.ContinueWith (_ => {
							BeginInvokeOnMainThread (() => {
								tableView.ReloadData ();
								
								var detailsController = controller.ParentViewController as AssignmentDetailsController;
								detailsController.UpdateAssignment ();

								var menuController = detailsController.ParentViewController.ChildViewControllers[1] as MenuController;
								menuController.UpdateAssignment ();
							});
						});
				}
			};
			alertView.Show();
		}
	}
}
