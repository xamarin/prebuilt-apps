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
using System.Threading.Tasks;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using UIKit;
using MapKit;
using CoreLocation;

using FieldService.Utilities;
using FieldService.Data;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// The controller for the main map with all the assignments on it
	/// </summary>
	public partial class MainMapController : BaseController
	{
		CLLocationManager locationManager;
		CLLocationCoordinate2D locationCoordinate;
		CLAuthorizationStatus authorizationStatus;

		bool activeAssignmentVisible = true;
		readonly AssignmentViewModel assignmentViewModel;

		public MainMapController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			//Hook up viewModel events
			assignmentViewModel.HoursChanged += OnHoursChanged;
			assignmentViewModel.RecordingChanged += OnRecordingChanged;
		}

		void OnHoursChanged (object sender, EventArgs e)
		{
			if (IsViewLoaded)
				timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
		}

		void OnRecordingChanged (object sender, EventArgs e)
		{
			if (IsViewLoaded)
				record.SetImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);

			//Do this because the ViewModel hangs around for the lifetime of the app
			assignmentViewModel.HoursChanged -= OnHoursChanged;
			assignmentViewModel.RecordingChanged -= OnRecordingChanged;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetupLocationManager ();

			//Setup mapView
			mapView.Delegate = new MapViewDelegate(this);

			//Setup other UI
			assignmentButton.SetBackgroundImage (Theme.AssignmentActive, UIControlState.Normal);
			contact.IconImage = Theme.IconPhone;
			address.IconImage = Theme.Map;
			priority.TextColor = UIColor.White;
			priorityBackground.Image = Theme.NumberBox;
			record.SetImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);
			
			timerLabel.TextColor =
				numberAndDate.TextColor =
				titleLabel.TextColor =
				startAndEnd.TextColor = Theme.LabelColor;
			
			status.StatusChanged += (sender, e) => {
				assignmentViewModel
					.SaveAssignmentAsync (assignmentViewModel.ActiveAssignment)
					.ContinueWith (_ => BeginInvokeOnMainThread (LoadActiveAssignment));
			};
			
			//Start the active assignment out as not visible
			SetActiveAssignmentVisible (false, false);

			if (!Theme.IsiOS7) {
				assignmentButton.SetBackgroundImage (Theme.AssignmentActiveBlue, UIControlState.Highlighted);
				toolbarShadow.Image = Theme.ToolbarShadow;
				timerBackgroundImage.Image = Theme.TimerField;
				return;
			}

			timerLabel.Font = Theme.FontOfSize (16f);
			priority.Font = Theme.FontOfSize (18f);
			startAndEnd.Font = Theme.BoldFontOfSize (10f);
			startAndEnd.TextColor = UIColor.White;

			//Shadow frame
			var frame = toolbarShadow.Frame;
			frame.Height = 1f;
			toolbarShadow.Frame = frame;
			toolbarShadow.Image = UIColor.LightGray.ToImage ();

			//Status dropdown frame
			frame = status.Frame;
			frame.Width /= 2f;
			frame.X += frame.Width + 9f;
			status.Frame = frame;

			const float offset = 100f;

			//Timer frame
			frame = timerLabel.Frame;
			frame.X += offset + 35f;
			timerLabel.Frame = frame;

			//Record (play/pause) button frame
			frame = record.Frame;
			frame.X += offset;
			record.Frame = frame;

			//Priority frames
			frame = priorityBackground.Frame;
			frame.X -= 10f;
			frame.Width = frame.Height;
			priorityBackground.Frame =
				priority.Frame = frame;

			//Info frames
			frame = numberAndDate.Frame;
			frame.X -= 10f;
			numberAndDate.Frame = frame;

			frame = titleLabel.Frame;
			frame.X -= 10f;
			titleLabel.Frame = frame;

			frame = startAndEnd.Frame;
			frame.X -= 6f;
			startAndEnd.Frame = frame;

			//Address frame
			frame = address.Frame;
			frame.X -= 10f;
			address.Frame = frame;

			//Contact frame
			frame = contact.Frame;
			frame.X -= 10f;
			contact.Frame = frame;

			//Assignment
			frame = activeAssignment.Frame;
			frame.Height -= 5f;
			activeAssignment.Frame = frame;

			//Additional green rectangle on the right
			var statusView = new UIView (new CGRect (activeAssignment.Frame.Width - 8f, 0f, 8f, activeAssignment.Frame.Height)) {
				BackgroundColor = Theme.GreenColor,
				AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleLeftMargin,
			};
			activeAssignment.AddSubview (statusView);

			//Additional box for the start/end date
			frame = startAndEnd.Frame;
			frame.X -= 4f;
			frame.Y += 4f;
			frame.Width = 102f;
			frame.Height = 16f;
			var timeBox = new UIImageView (frame) {
				Image = Theme.TimeBox,
				ContentMode = UIViewContentMode.Left,
			};
			activeAssignment.AddSubview (timeBox);
			activeAssignment.BringSubviewToFront (startAndEnd);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Clear and reload our map placemarks
			mapView.ClearPlacemarks ();

			List<MKPlacemark> placemarks = new List<MKPlacemark>();
			if (assignmentViewModel.ActiveAssignment != null) {
				var assignment = assignmentViewModel.ActiveAssignment;
				if (assignment.Latitude != 0 && assignment.Longitude != 0)
					placemarks.Add (assignment.ToPlacemark ());
			}
			
			foreach (var assignment in assignmentViewModel.Assignments)
				if (assignment.Latitude != 0 && assignment.Longitude != 0)
					placemarks.Add (assignment.ToPlacemark ());
			
			if (placemarks.Count > 0)
				mapView.AddAnnotations (placemarks.ToArray ());

			//Show/hide the active assignment
			LoadActiveAssignment ();

			//Apply the current orientation
			if (InterfaceOrientation.IsLandscape ()) {
				contact.Alpha = 
					address.Alpha = 1;
			} else {
				contact.Alpha = 
					address.Alpha = 0;
			}
		}

		void SetupLocationManager ()
		{
			if (locationManager != null)
				return;

			locationManager = new CLLocationManager ();
			locationManager.AuthorizationChanged += OnAuthorizationChanged;
		}

		void OnAuthorizationChanged (object sender, CLAuthorizationChangedEventArgs e)
		{
			authorizationStatus = e.Status;

			switch (authorizationStatus) {
			case CLAuthorizationStatus.AuthorizedAlways:
				locationManager.StartUpdatingLocation ();
				break;

			case CLAuthorizationStatus.NotDetermined:
				locationManager.RequestAlwaysAuthorization ();
				break;

			default:
				break;
			}
		}

		/// <summary>
		/// Sets the visibility of the active assignment, with a nice animation
		/// </summary>
		void SetActiveAssignmentVisible (bool visible, bool animate = true)
		{
			if (visible == activeAssignmentVisible)
				return;

			if (animate) {
				UIView.BeginAnimations ("ChangeActiveAssignment");
				UIView.SetAnimationDuration (.3);
				UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
			}

			//Modify the mapViews's frame
			float height = 95f;
			var frame = mapView.Frame;
			if (visible) {
				frame.Y += height;
				frame.Height -= height;
			} else {
				frame.Y -= height;
				frame.Height += height;
			}
			mapView.Frame = frame;

			//Modify the active assignment's frame
			frame = activeAssignment.Frame;
			if (visible)
				frame.Y += height;
			else
				frame.Y -= height;

			activeAssignment.Frame = frame;

			//Modify the toolbar shadow's frame
			frame = toolbarShadow.Frame;
			if (visible)
				frame.Y += height;
			else
				frame.Y -= height;

			toolbarShadow.Frame = frame;

			if (animate)
				UIView.CommitAnimations ();

			activeAssignmentVisible = visible;
		}

		/// <summary>
		/// Sets the active assignment's views
		/// </summary>
		void LoadActiveAssignment ()
		{
			if (assignmentViewModel.ActiveAssignment == null) {
				SetActiveAssignmentVisible (false);
				return;
			}

			SetActiveAssignmentVisible (true);
			var assignment = assignmentViewModel.ActiveAssignment;

			//Update font size on priority
			priority.Font = Theme.FontOfSize (assignment.Priority >= 10 ? 14f : 18f);

			priority.Text = assignment.Priority.ToString ();
			numberAndDate.Text = string.Format ("#{0} {1}", assignment.JobNumber, assignment.StartDate.Date.ToShortDateString ());
			titleLabel.Text = assignment.CompanyName;
			startAndEnd.Text = assignment.FormatStartEndDates ();
			contact.TopLabel.Text = assignment.ContactName;
			contact.BottomLabel.Text = assignment.ContactPhone;
			address.TopLabel.Text = assignment.Address;
			address.BottomLabel.Text = string.Format ("{0}, {1} {2}", assignment.City, assignment.State, assignment.Zip);
			status.Assignment = assignment;
		}

		/// <summary>
		/// We override this to show/hide some controls during rotation
		/// </summary>c
		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			bool wasPortrait = InterfaceOrientation.IsPortrait ();
			bool willBePortrait = toInterfaceOrientation.IsPortrait ();
			bool wasLandscape = InterfaceOrientation.IsLandscape ();
			bool willBeLandscape = toInterfaceOrientation.IsLandscape ();
			
			if (wasPortrait && willBeLandscape)
				SetContactVisible (true, duration);
			else if (wasLandscape && willBePortrait)
				SetContactVisible (false, duration);
			
			base.WillRotate (toInterfaceOrientation, duration);
		}
		
		/// <summary>
		/// This is uses to show/hide contact and address on rotation
		/// </summary>
		void SetContactVisible (bool visible, double duration)
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
		/// Event when record button is pressed
		/// </summary>
		partial void Record ()
		{
			record.Enabled = false;
			Task task = assignmentViewModel.Recording ? assignmentViewModel.PauseAsync () : assignmentViewModel.RecordAsync ();
			task.ContinueWith (_ => BeginInvokeOnMainThread (() => record.Enabled = true));
		}
		
		/// <summary>
		/// Event when the address is clicked on the active assignment
		/// </summary>
		partial void Address ()
		{
			var menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
			menuViewModel.MenuIndex = SectionIndex.Maps;
			assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
			PerformSegue ("AssignmentDetails", this);
		}

		/// <summary>
		/// Event when the active assignment is clicked
		/// </summary>
		partial void ActiveAssignmentSelected ()
		{
			var menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
			menuViewModel.MenuIndex = SectionIndex.Maps;
			assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
			PerformSegue ("AssignmentDetails", this);
		}

		/// <summary>
		/// Delegate for the map view
		/// </summary>
		class MapViewDelegate : MKMapViewDelegate
		{
			const string Identifier = "AssignmentAnnotation";
			readonly MainMapController controller;
			readonly AssignmentViewModel assignmentViewModel;
			
			public MapViewDelegate (MainMapController controller)
			{
				this.controller = controller;
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			}
			
			/// <summary>
			/// Returns our custom MKAnnotationView
			/// </summary>
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
			{
				if (annotation is MKUserLocation)
					return null;

				var annotationView = mapView.DequeueReusableAnnotation (Identifier) as MKPinAnnotationView;
				if (annotationView == null)
					annotationView = new MKPinAnnotationView (annotation, Identifier) {
						PinColor = MKPinAnnotationColor.Green,
						AnimatesDrop = true,
						CanShowCallout = true,
						RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure)
					};
				else
					annotationView.Annotation = annotation;

				return annotationView;
			}
			
			/// <summary>
			/// This is the callback for when the detail disclosure is clicked
			/// </summary>
			public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
			{
				var menuViewModel = ServiceContainer.Resolve<MenuViewModel>();
				menuViewModel.MenuIndex = SectionIndex.Summary;
				assignmentViewModel.SelectedAssignment = GetAssignment (view.Annotation as MKPlacemark);
				controller.PerformSegue ("AssignmentDetails", controller);
			}
			
			/// <summary>
			/// Center the map when the user is located
			/// </summary>
			public override void DidUpdateUserLocation (MKMapView mapView, MKUserLocation userLocation)
			{
				if (userLocation != null) {
					var span = new MKCoordinateSpan (15, 15);
					var region = new MKCoordinateRegion (userLocation.Coordinate, span);
					mapView.SetRegion (region, true);
				}
			}
			
			/// <summary>
			/// This pulls out an assignment we placed in a MKPlacemark
			/// </summary>
			Assignment GetAssignment (MKPlacemark annotation)
			{
				return annotation.AddressDictionary[new NSString("Assignment")].UnwrapObject<Assignment>();
			}
		}
	}
}
