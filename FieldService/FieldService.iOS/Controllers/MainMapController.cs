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
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using FieldService.Utilities;
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// The controller for the main map with all the assignments on it
	/// </summary>
	public partial class MainMapController : BaseController
	{
		readonly AssignmentsController assignmentController;

		public MainMapController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			TabBarItem.Image = Theme.MapIcon;

			assignmentController = ServiceContainer.Resolve<AssignmentsController>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Setup mapView
			mapView.Delegate = new MapViewDelegate(this, assignmentController);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			var assignmentViewModel = assignmentController.AssignmentViewModel;
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
		}

		/// <summary>
		/// Delegate for the map view
		/// </summary>
		private class MapViewDelegate : MKMapViewDelegate
		{
			const string Identifier = "AssignmentAnnotation";
			readonly MainMapController mapController;
			readonly AssignmentsController assignmentController;
			
			public MapViewDelegate (MainMapController mapController, AssignmentsController assignmentController)
			{
				this.mapController = mapController;
				this.assignmentController = assignmentController;
			}
			
			/// <summary>
			/// Returns our custom MKAnnotationView
			/// </summary>
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
			{
				if (annotation is MKUserLocation) {
					return null;
				} else {
					var annotationView = mapView.DequeueReusableAnnotation (Identifier) as MKPinAnnotationView;
					if (annotationView == null) {
						annotationView = new MKPinAnnotationView(annotation, Identifier);
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
				assignmentController.Assignment = GetAssignment (view.Annotation as MKPlacemark);
				mapController.PerformSegue ("AssignmentDetails", mapController);
			}
			
			/// <summary>
			/// Center the map when the user is located
			/// </summary>
			public override void DidUpdateUserLocation (MKMapView mapView, MKUserLocation userLocation)
			{
				var span = new MKCoordinateSpan(15, 15);
				var region = new MKCoordinateRegion(userLocation.Coordinate, span);
				mapView.SetRegion (region, true);
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
