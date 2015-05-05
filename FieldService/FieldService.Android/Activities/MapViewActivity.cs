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

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Common;
using Android.Gms.Maps.Model;

using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Extensions = FieldService.Android.Utilities.AndroidExtensions;

namespace FieldService.Android
{
	/// <summary>
	/// Activity for the map overview
	/// </summary>
	[Activity (Label = "Map View", Theme = "@style/CustomHoloTheme")]
	public class MapViewActivity : BaseActivity, IOnMapReadyCallback
	{
		const int ConnectionFailureResolutionRequest = 9000;

		readonly AssignmentViewModel assignmentViewModel;
		readonly MenuViewModel menuViewModel;

		int initTry = 0;

		MapView mapView;
		LinearLayout assignmentMapViewLayout, buttonLayout, timerLayout;
		ToggleButton timer;
		Spinner activeSpinner;
		Assignment assignment;
		ImageView spinnerImage;
		TextView number, name, job, phone, address, timerText;
		AssignmentTabActivity.MapDataWrapper mapData;
		RelativeLayout phoneButton, mapButton;
		GoogleMap googleMap;

		public MapViewActivity ()
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
			assignmentViewModel.HoursChanged += HoursChanged;
			menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();
		}

		public void OnMapReady (GoogleMap map)
		{
			googleMap = map;
			googleMap.UiSettings.CompassEnabled = false;
			googleMap.UiSettings.MyLocationButtonEnabled = false;
			googleMap.UiSettings.MapToolbarEnabled = false;

			UpdateLocations ();
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.MapViewLayout);

			assignmentMapViewLayout = FindViewById<LinearLayout> (Resource.Id.mapViewAssignmentLayout);
			assignmentMapViewLayout.Click += (sender, e) => {
				var intent = new Intent (this, typeof(SummaryActivity));
				var tabActivity = (AssignmentTabActivity)Parent;
				tabActivity.MapData = null;
				assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
				menuViewModel.MenuIndex = Constants.Navigation.IndexOf ("Map");
				StartActivity (intent);
			};
	    
			mapView = FindViewById<MapView> (Resource.Id.googleMapsView);
			mapView.OnCreate (bundle);
			mapView.GetMapAsync (this);

			//View containing the active assignment
			var view = new View (this);
			LayoutInflater inflator = (LayoutInflater)GetSystemService (Context.LayoutInflaterService);
			view = inflator.Inflate (Resource.Layout.AssignmentItemLayout, null);
			assignmentMapViewLayout.AddView (view);
			view.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			view.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.active_assignment_selector));
			number = view.FindViewById<TextView> (Resource.Id.assignmentItemNumber);
			job = view.FindViewById<TextView> (Resource.Id.assignmentJob);
			name = view.FindViewById<TextView> (Resource.Id.assignmentName);
			phone = view.FindViewById<TextView> (Resource.Id.assignmentPhone);
			address = view.FindViewById<TextView> (Resource.Id.assignmentAddress);
			buttonLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentButtonLayout);
			timerLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentTimerLayout);
			activeSpinner = view.FindViewById<Spinner> (Resource.Id.assignmentStatus);
			spinnerImage = view.FindViewById<ImageView> (Resource.Id.assignmentStatusImage);
			timer = view.FindViewById<ToggleButton> (Resource.Id.assignmentTimer);
			timerText = view.FindViewById<TextView> (Resource.Id.assignmentTimerText);
			phoneButton = view.FindViewById<RelativeLayout> (Resource.Id.assignmentPhoneLayout);
			mapButton = view.FindViewById<RelativeLayout> (Resource.Id.assignmentAddressLayout);

			phoneButton.Click += (sender, e) => {
				Extensions.MakePhoneCall (this, phone.Text);
			};

			mapButton.Click += (sender, e) => {
				var intent = new Intent (this, typeof(SummaryActivity));
				var tabActivity = (AssignmentTabActivity)Parent;
				tabActivity.MapData = null;
				assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
				menuViewModel.MenuIndex = 0;
				StartActivity (intent);
			};

			assignmentViewModel.LoadTimerEntryAsync ().ContinueWith (_ => {
				RunOnUiThread (() => {
					timer.Checked = assignmentViewModel.Recording;
				});
			});

			timer.CheckedChange += (sender, e) => {
				if (e.IsChecked != assignmentViewModel.Recording) {
					if (assignmentViewModel.Recording)
						assignmentViewModel.PauseAsync ();
					else
						assignmentViewModel.RecordAsync ();
				}
			};

			activeSpinner.ItemSelected += (sender, e) => {

				if (assignment == null)
					return;
				
				var selected = assignmentViewModel.AvailableStatuses.ElementAtOrDefault (e.Position);
				if (selected != assignment.Status) {
					switch (selected) {
					case AssignmentStatus.Hold:
						assignment.Status = selected;
						assignmentViewModel.SaveAssignmentAsync (assignment).ContinueWith (_ => {
							RunOnUiThread (() => {
								SetAssignment (false);
								UpdateLocations ();
							});
						});
						break;
					case AssignmentStatus.Complete:
						var intent = new Intent (this, typeof(SummaryActivity));
						menuViewModel.MenuIndex = Constants.Navigation.IndexOf (Constants.Confirmations);
						StartActivity (intent);
						break;
					default:
						break;
					}
				}
			};
		}

		/// <summary>
		/// Enabling my location and setting active assignment
		/// </summary>
		protected override void OnResume ()
		{
			base.OnResume ();
			mapView.OnResume ();
			SetAssignment (assignmentViewModel.ActiveAssignment != null);
			UpdateLocations ();
		}

		/// <summary>
		/// Updates location pins on the map.
		/// </summary>
		async void UpdateLocations ()
		{
			if (googleMap == null)
				return;

			googleMap.Clear ();

			try {
				MapsInitializer.Initialize (this);
			} catch (GooglePlayServicesNotAvailableException e) {
				Console.WriteLine ("Google Play Services not available:" + e);
				return;
			}
			
			await assignmentViewModel.LoadAssignmentsAsync ();

			var points = new LatLngBounds.Builder ();

			foreach (var assignment in assignmentViewModel.Assignments) {
				var markerOptions = GetMarkerOptionsForAssignment (assignment);
				googleMap.AddMarker (markerOptions);
				points.Include (markerOptions.Position);
			}

			if (assignmentViewModel.ActiveAssignment != null) {
				var markerOptions = GetMarkerOptionsForAssignment (assignmentViewModel.ActiveAssignment);
				googleMap.AddMarker (markerOptions).ShowInfoWindow ();
				points.Include (markerOptions.Position);
				googleMap.CameraPosition.Target = markerOptions.Position;
			}

			var bounds = points.Build ();

			if (mapView.Width == 0) {
				initTry = 0;
				PostDelayInitMap (bounds);
			} else {
				googleMap.MoveCamera (CameraUpdateFactory.NewLatLngBounds (bounds, 0));
			}
		}

		void PostDelayInitMap (LatLngBounds bounds)
		{
			if (initTry == 4)
				return;

			mapView.PostDelayed (() => {
				initTry++;
				if (mapView.Width == 0)
					PostDelayInitMap (bounds);
				else
					googleMap.MoveCamera (CameraUpdateFactory.NewLatLngBounds (bounds, 0));
			}, 250);
		}

		MarkerOptions GetMarkerOptionsForAssignment (Assignment assignment)
		{
			var markerOptions = new MarkerOptions ();
			markerOptions.SetPosition (new LatLng (assignment.Latitude, assignment.Longitude));
			markerOptions.SetTitle (assignment.CompanyName);
			markerOptions.SetSnippet (string.Format ("{0} {1}, {2} {3}",
				assignment.Address, assignment.City, assignment.State, assignment.Zip));
			
			return markerOptions;
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			var tabActivity = (AssignmentTabActivity)Parent;
			if (mapView.ChildCount > 0) {
				if (mapData == null) {
					mapData = new AssignmentTabActivity.MapDataWrapper ();
				}
				mapData.OverlayBubble = mapView.GetChildAt (0);
				mapData.Zoom = 10;
			}
			if (tabActivity != null) {
				tabActivity.MapData = mapData;
			}
			base.OnSaveInstanceState (outState);
		}

		/// <summary>
		/// Clearing overlays on map, stopping my location, clearing active assignment in the layout.
		/// </summary>
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			mapView.OnDestroy ();
		}

		/// <summary>
		/// Clearing overlay bubble when you leave the screen.
		/// </summary>
		protected override void OnPause ()
		{
			base.OnPause ();
			mapView.OnPause ();
		}

		/// <summary>
		/// Hours Changed event for tracking time on active assignment
		/// </summary>
		void HoursChanged (object sender, EventArgs e)
		{
			if (timerText == null)
				return;

			RunOnUiThread (() => {
				timerText.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
			});
		}

		/// <summary>
		/// Sets the current active assignment
		/// </summary>
		/// <param name="visible"></param>
		void SetAssignment (bool visible)
		{
			if (!visible) {
				assignmentMapViewLayout.Visibility = ViewStates.Gone;
				return;
			}

			assignmentMapViewLayout.Visibility = ViewStates.Visible;
			assignment = assignmentViewModel.ActiveAssignment;

			buttonLayout.Visibility = ViewStates.Gone;
			timerLayout.Visibility = ViewStates.Visible;

			var adapter = new SpinnerAdapter<AssignmentStatus> (assignmentViewModel.AvailableStatuses, this, Resource.Layout.SimpleSpinnerItem);
			adapter.TextColor = Resources.GetColor (Resource.Color.greyspinnertext);
			adapter.Background = Resources.GetColor (Resource.Color.assignmentblue);
			activeSpinner.Adapter = adapter;
			activeSpinner.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (assignment.Status));
			activeSpinner.SetBackgroundResource (Resource.Drawable.triangleblue);
			spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);

			number.Text = assignment.Priority.ToString ();
			job.Text = string.Format ("#{0} {1}\n{2}", assignment.JobNumber, assignment.StartDate.ToShortDateString (), assignment.CompanyName);
			name.Text = assignment.ContactName;
			phone.Text = assignment.ContactPhone;
			address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
			timerText.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
		}
	}
}