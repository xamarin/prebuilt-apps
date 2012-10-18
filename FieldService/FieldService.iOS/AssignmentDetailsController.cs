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
	public partial class AssignmentDetailsController : UIViewController
	{
		readonly AssignmentViewModel assignmentViewModel;
		UIView lastSelectedView;
		MKMapView mapView;

		public AssignmentDetailsController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
		}

		/// <summary>
		/// Gets or sets the assignment to be shown
		/// </summary>
		public Assignment Assignment {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI that is required to be setup from code
			lastSelectedView = container;
			View.BackgroundColor = Theme.LinenPattern;
			assignmentBackground.Image = Theme.AssignmentActive;
			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;
			priority.TextColor = UIColor.White;
			priorityBackground.Image = Theme.NumberBox;
			descriptionBackground.Image = Theme.Row180End;
			accept.SetBackgroundImage (Theme.Accept, UIControlState.Normal);
			decline.SetBackgroundImage (Theme.Decline, UIControlState.Normal);
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
			var viewHistory = new UIBarButtonItem("View History", UIBarButtonItemStyle.Bordered, delegate {	});
			viewHistory.SetBackgroundImage (Theme.BlueNavButton, UIControlState.Normal, UIBarMetrics.Default);
			viewHistory.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
			toolbar.Items = new UIBarButtonItem[] { descriptionButton, new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), viewHistory };

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
		public void SectionSelected(int index) 
		{
			UIView nextView = null;
			switch (index) {
			case 0:
				//Summary
				nextView = container;
				break;
			case 1:
				//Map
				if (mapView == null) {
					mapView = new MKMapView();
					mapView.ShowsUserLocation = true;
					mapView.AutoresizingMask = container.AutoresizingMask;
				}

				nextView = mapView;
				break;
			default:
				return; //This means this section isn't done yet
			}

			nextView.Frame = lastSelectedView.Frame;
			UIView.Transition (lastSelectedView, nextView, .3, UIViewAnimationOptions.TransitionCrossDissolve, () => {
				lastSelectedView = nextView;
				UpdateAssignment ();
			});
		}

		/// <summary>
		/// Sets up the UI for the assignment
		/// </summary>
		private void UpdateAssignment ()
		{
			if (Assignment != null && IsViewLoaded) {

				priority.Text = Assignment.Priority.ToString ();
				numberAndDate.Text = string.Format ("#{0} {1}", Assignment.JobNumber, Assignment.StartDate.Date.ToShortDateString ());
				title.Text = Assignment.Title;
				startAndEnd.Text = string.Format ("Start: {0} End: {1}", Assignment.StartDate.ToShortTimeString (), Assignment.EndDate.ToShortTimeString ());
				contact.TopLabel.Text = Assignment.ContactName;
				contact.BottomLabel.Text = Assignment.ContactPhone;
				address.TopLabel.Text = Assignment.Address;
				address.BottomLabel.Text = string.Format ("{0}, {1} {2}", Assignment.City, Assignment.State, Assignment.Zip);
				status.Assignment = Assignment;

				if (Assignment.Status == AssignmentStatus.New) {
					status.Hidden = true;
					decline.Hidden =
						accept.Hidden = false;
				} else {
					status.Hidden = false;
					decline.Hidden =
						accept.Hidden = true;
				}

				if (container == lastSelectedView) {
					description.Text = Assignment.Description;
					descriptionTitle.Text = Assignment.Title;
					items.Text = Assignment.TotalItems.ToString ();
					hours.Text = Assignment.TotalHours.TotalHours.ToString ("0.0");
					expenses.Text = Assignment.TotalExpenses.ToString ("$0.00");
				} else if (mapView == lastSelectedView) {
					mapView.ClearPlacemarks ();
					mapView.AddPlacemark (Assignment.ToPlacemark());
				}
			}
		}

		/// <summary>
		/// Event when accept button is clicked
		/// </summary>
		partial void Accept ()
		{
			Assignment.Status = AssignmentStatus.Hold;

			SaveAssignment ();
		}

		/// <summary>
		/// Event when decline button is clicked
		/// </summary>
		partial void Decline ()
		{
			Assignment.Status = AssignmentStatus.Declined;
			
			SaveAssignment ();
		}

		/// <summary>
		/// Saves the assignment.
		/// </summary>
		private void SaveAssignment ()
		{
			assignmentViewModel.SaveAssignment (Assignment).ContinueOnUIThread (t => UpdateAssignment ());
		}
	}
}
