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
using System.Linq;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using FieldService.ViewModels;
using FieldService.Data;
using FieldService.Utilities;
using MonoTouch.CoreLocation;
using MonoTouch.AddressBook;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for the main list of assignments
	/// </summary>
	public partial class AssignmentsController : BaseController
	{
		readonly AssignmentViewModel assignmentViewModel;
		bool activeAssignmentVisible = true;
		MKMapView mapView;

		public AssignmentsController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
			assignmentViewModel.HoursChanged += (sender, e) => {
				if (IsViewLoaded) {
					timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
				}
			};
			assignmentViewModel.RecordingChanged += (sender, e) => {
				if (IsViewLoaded) {
					record.SetBackgroundImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);
				}
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Setup UI that is required from code
			View.BackgroundColor = Theme.LinenPattern;
			tableView.Source = new TableSource (this);
			assignmentButton.SetBackgroundImage (Theme.AssignmentActive, UIControlState.Normal);
			assignmentButton.SetBackgroundImage (Theme.AssignmentActiveBlue, UIControlState.Highlighted);
			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;
			priority.TextColor = UIColor.White;
			priorityBackground.Image = Theme.NumberBox;
			record.SetBackgroundImage (Theme.Record, UIControlState.Normal);
			timerBackgroundImage.Image = Theme.TimerField;
			toolbarShadow.Image = Theme.ToolbarShadow;
			settings.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			status.StatusChanged += (sender, e) => {
				assignmentViewModel
					.SaveAssignment (assignmentViewModel.ActiveAssignment)
					.ContinueOnUIThread (_ => ReloadAssignments ());
			};

			//Start the active assignment out as not visible
			SetActiveAssignmentVisible (false, false);

			//Load the current timer status
			record.Enabled = false;
			assignmentViewModel.LoadTimerEntry ().ContinueOnUIThread (_ => {
				record.Enabled = true;
				if (assignmentViewModel.Recording) {
					record.SetBackgroundImage (Theme.RecordActive, UIControlState.Normal);
				} else {
					record.SetBackgroundImage (Theme.Record, UIControlState.Normal);
				}
			});
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Load our assignments
			ReloadAssignments ();
			//Apply the current orientation
			if (InterfaceOrientation.IsLandscape ()) {
				contact.Alpha = 
					address.Alpha = 1;
			} else {
				contact.Alpha = 
					address.Alpha = 0;
			}
		}

		/// <summary>
		/// We override this to show/hide some controls during rotation
		/// </summary>
		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			bool wasPortrait = InterfaceOrientation.IsPortrait ();
			bool willBePortrait = toInterfaceOrientation.IsPortrait ();
			bool wasLandscape = InterfaceOrientation.IsLandscape ();
			bool willBeLandscape = toInterfaceOrientation.IsLandscape ();

			if (wasPortrait && willBeLandscape) {
				SetContactVisible (true, duration);
			} else if (wasLandscape && willBePortrait) {
				SetContactVisible (false, duration);
			}

			base.WillRotate (toInterfaceOrientation, duration);
		}

		/// <summary>
		/// This is uses to show/hide contact and address on rotation
		/// </summary>
		private void SetContactVisible (bool visible, double duration)
		{
			UIView.BeginAnimations ("SetContactVisible");
			UIView.SetAnimationDuration (duration);
			UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);

			if (visible) {
				contact.Alpha = 
					address.Alpha = 1;
			} else {
				contact.Alpha = 
					address.Alpha = 0;
			}

			UIView.CommitAnimations ();
		}

		/// <summary>
		/// Reloads the entire list of assignments
		/// </summary>
		public void ReloadAssignments ()
		{
			assignmentViewModel.LoadAssignments ().ContinueOnUIThread (_ => {
				if (assignmentViewModel.ActiveAssignment == null) {
					SetActiveAssignmentVisible (false);
				} else {
					SetActiveAssignmentVisible (true);
					LoadActiveAssignment ();
				}
				
				tableView.ReloadData ();
			});
		}

		/// <summary>
		/// Reloads a single row
		/// </summary>
		public void ReloadSingleRow (NSIndexPath indexPath)
		{
			tableView.ReloadRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
		}

		/// <summary>
		/// Event when the active assignment is clicked
		/// </summary>
		partial void ActiveAssignmentSelected ()
		{
			var assignmentController = ServiceContainer.Resolve <AssignmentDetailsController>();
			assignmentController.Assignment = assignmentViewModel.ActiveAssignment;
			Theme.TransitionController <MainController>();
		}

		/// <summary>
		/// Sets the visibility of the active assignment, with a nice animation
		/// </summary>
		private void SetActiveAssignmentVisible (bool visible, bool animate = true)
		{
			if (visible != activeAssignmentVisible) {
				if (animate) {
					UIView.BeginAnimations ("ChangeActiveAssignment");
					UIView.SetAnimationDuration (.3);
					UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
				}

				activeAssignment.Alpha = visible ? 1 : 0;

				//Modify the tableView's frame
				float height = 95;
				var frame = tableView.Frame;
				if (visible) {
					frame.Y += height;
					frame.Height -= height;
				} else {
					frame.Y -= height;
					frame.Height += height;
				}
				tableView.Frame = frame;

				//Modify the toolbar shadow
				frame = toolbarShadow.Frame;
				if (visible) {
					frame.Y += height;
				} else {
					frame.Y -= height;
				}
				toolbarShadow.Frame = frame;

				//Now apply to mapView, if it has been created
				if (mapView != null) {
					frame = mapView.Frame;
					if (visible) {
						frame.Y += height;
						frame.Height -= height;
					} else {
						frame.Y -= height;
						frame.Height += height;
					}
					mapView.Frame = frame;
				}

				if (animate) {
					UIView.CommitAnimations ();
				}
				activeAssignmentVisible = visible;
			}
		}

		/// <summary>
		/// Sets the active assignment's views
		/// </summary>
		private void LoadActiveAssignment ()
		{
			var assignment = assignmentViewModel.ActiveAssignment;
			priority.Text = assignment.Priority.ToString ();
			numberAndDate.Text = string.Format ("#{0} {1}", assignment.JobNumber, assignment.StartDate.Date.ToShortDateString ());
			title.Text = assignment.Title;
			startAndEnd.Text = string.Format ("Start: {0} End: {1}", assignment.StartDate.ToShortTimeString (), assignment.EndDate.ToShortTimeString ());
			contact.TopLabel.Text = assignment.ContactName;
			contact.BottomLabel.Text = assignment.ContactPhone;
			address.TopLabel.Text = assignment.Address;
			address.BottomLabel.Text = string.Format ("{0}, {1} {2}", assignment.City, assignment.State, assignment.Zip);
			status.Assignment = assignment;
		}

		/// <summary>
		/// Event when record button is pressed
		/// </summary>
		partial void Record ()
		{
			record.Enabled = false;
			if (assignmentViewModel.Recording) {
				assignmentViewModel.Pause ().ContinueOnUIThread (t => record.Enabled = true);
			} else {
				assignmentViewModel.Record ().ContinueOnUIThread (t => record.Enabled = true);
			}
		}

		/// <summary>
		/// Event when the settings toolbar item is clicked
		/// </summary>
		partial void Settings (NSObject sender)
		{

		}

		/// <summary>
		/// Event when user changes the current tab
		/// </summary>
		partial void TabChanged ()
		{
			if (segmentedControl.SelectedSegment == 1) {
				if (mapView == null) {
					mapView = new MKMapView (tableView.Frame) { 
						ShowsUserLocation = true,
						AutoresizingMask = tableView.AutoresizingMask,
						Delegate = new MapViewDelegate(),
					};
				} else {
					//This fixes a bug when rotating and flipping views
					mapView.Frame = tableView.Frame;
				}


				UIView.Transition (tableView, mapView, .3, UIViewAnimationOptions.TransitionCurlUp, delegate {
					mapView.ClearPlacemarks ();

					List<MKPlacemark> placemarks = new List<MKPlacemark>();
					if (assignmentViewModel.ActiveAssignment != null) {
						var assignment = assignmentViewModel.ActiveAssignment;
						if (assignment.Latitude != 0 && assignment.Longitude != 0)
							placemarks.Add (assignment.ToPlacemark ());
					}

					foreach (var assignment in assignmentViewModel.Assignments) {
						if (assignment.Latitude != 0 && assignment.Longitude != 0)
							placemarks.Add (assignment.ToPlacemark ());
					}

					if (placemarks.Count > 0)
						mapView.AddPlacemarks (placemarks.ToArray ());
				});
			} else {
				//This fixes a bug when rotating and flipping views
				tableView.Frame = mapView.Frame;

				UIView.Transition (mapView, tableView, .3, UIViewAnimationOptions.TransitionCurlDown, delegate {
					ReloadAssignments ();
				});
			}

			//Bring the toolbarShadow to the front, UIView.Transition() puts it behind
			View.BringSubviewToFront (toolbarShadow);
		}

		/// <summary>
		/// Data source for the tableView of assignments
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly AssignmentViewModel assignmentViewModel;
				
			public TableSource (AssignmentsController controller)
			{
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
			}

			public override int RowsInSection (UITableView tableView, int section)
			{
				return assignmentViewModel.Assignments == null ? 0 : assignmentViewModel.Assignments.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var assignment = assignmentViewModel.Assignments [indexPath.Row];
				var cell = tableView.DequeueReusableCell ("AssignmentCell") as AssignmentCell;
				cell.SetAssignment (assignment, indexPath);
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var assignmentController = ServiceContainer.Resolve <AssignmentDetailsController>();
				assignmentController.Assignment = assignmentViewModel.Assignments[indexPath.Row];
				Theme.TransitionController <MainController>();
			}
		}

		/// <summary>
		/// Delegate for the map view
		/// </summary>
		private class MapViewDelegate : MKMapViewDelegate
		{
			const string UserIdentifier = "UserAnnotation";
			const string AssignmentIdentifier = "AssignmentAnnotation";
			readonly UIPopoverController popoverController;

			public MapViewDelegate ()
			{
				popoverController = new UIPopoverController(new UIViewController());
				popoverController.PopoverContentSize = new System.Drawing.SizeF(100, 100);
			}

			/// <summary>
			/// Returns our custom MKAnnotationView
			/// </summary>
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
			{
				var userLocation = annotation as MKUserLocation;
				if (userLocation != null ) {
					var annotationView = mapView.DequeueReusableAnnotation (UserIdentifier) as MKPinAnnotationView;
					if (annotationView == null) {
						annotationView = new MKPinAnnotationView(annotation, UserIdentifier);
						annotationView.CanShowCallout = true;
						annotationView.AnimatesDrop = false;
					} else {
						annotationView.Annotation = annotation;
					}
					return annotationView;
				} else {
					var annotationView = mapView.DequeueReusableAnnotation (AssignmentIdentifier) as MKPinAnnotationView;
					if (annotationView == null) {
						annotationView = new MKPinAnnotationView(annotation, AssignmentIdentifier);
						annotationView.PinColor = MKPinAnnotationColor.Green;
						annotationView.AnimatesDrop = true;
						annotationView.CanShowCallout = true;
						annotationView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);
					} else {
						annotationView.Annotation = annotation;
					}
					return annotationView;
				}
			}

			/// <summary>
			/// This is the callback for when the detail disclosure is clicked
			/// </summary>
			public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
			{
				var assignmentController = ServiceContainer.Resolve <AssignmentDetailsController>();
				assignmentController.Assignment = GetAssignment (view.Annotation as MKPlacemark);
				Theme.TransitionController <MainController>();
			}

			/// <summary>
			/// This pulls out an assignment we placed in a MKPlacemark
			/// </summary>
			private Assignment GetAssignment(MKPlacemark annotation)
			{
				return annotation.AddressDictionary[new NSString("Assignment")].UnwrapObject<Assignment>();
			}
		}
	}
}
