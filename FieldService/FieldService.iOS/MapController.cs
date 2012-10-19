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
using MonoTouch.MapKit;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for displaying the map for an assignment
	/// </summary>
	public class MapController : BaseController
	{
		readonly AssignmentDetailsController detailsController;
		MKMapView mapView;

		public MapController ()
		{
			detailsController = ServiceContainer.Resolve <AssignmentDetailsController>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Setup the map view
			mapView = new MKMapView(View.Frame);
			mapView.ShowsUserLocation = true;
			mapView.AutoresizingMask = View.AutoresizingMask;

			View.BackgroundColor = UIColor.Clear;
			View.AddSubview (mapView);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Load the placemark
			mapView.ClearPlacemarks ();
			mapView.AddPlacemark (detailsController.Assignment.ToPlacemark());
		}
	}
}

