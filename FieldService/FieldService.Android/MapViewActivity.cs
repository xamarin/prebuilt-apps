using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.GoogleMaps;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Android.Hardware;

namespace FieldService.Android {
    [Activity (Label = "Map View", Theme = "@style/CustomHoloTheme")]
    public class MapViewActivity : MapActivity {
        AssignmentViewModel assignmentViewModel;
        RelativeLayout assignmentMapViewLayout;
        MapView mapView;
        MyLocationOverlay myLocation;

        public MapViewActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Create your application here
            SetContentView (Resource.Layout.MapViewLayout);

            assignmentMapViewLayout = FindViewById<RelativeLayout> (Resource.Id.mapViewAssignmentLayout);
            mapView = FindViewById<MapView> (Resource.Id.googleMapsView);

            if (assignmentViewModel.ActiveAssignment != null) {
                SetAssignment (true);
            } else {
                SetAssignment (false);
            }

            myLocation = new MyLocationOverlay (this, mapView);
            myLocation.RunOnFirstFix (() => {
                mapView.Controller.AnimateTo (myLocation.MyLocation);
            });
            mapView.Controller.SetZoom (5);
            mapView.Clickable = true;
            mapView.Enabled = true;
            mapView.SetBuiltInZoomControls (true);
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            assignmentViewModel.LoadAssignmentsAsync ().ContinueOnUIThread (_ => {
                foreach (var item in assignmentViewModel.Assignments) {
                    //var overlay = new OverlayItem(new GeoPoint(item

                }
                myLocation.EnableMyLocation ();
            });
        }

        protected override void OnStop ()
        {
            myLocation.DisableMyLocation ();
            base.OnStop ();
        }

        protected override bool IsRouteDisplayed
        {
            get { return false; }
        }

        private void SetAssignment (bool visible)
        {
            if (visible) {
                View view = null;
                var assignment = assignmentViewModel.ActiveAssignment;
                //look at layouts children to get view
                if (assignmentMapViewLayout.ChildCount > 0) {
                    view = assignmentMapViewLayout.GetChildAt (0);

                } else {
                    view = new View (this);
                    LayoutInflater inflator = (LayoutInflater)GetSystemService (Context.LayoutInflaterService);
                    view = inflator.Inflate (Resource.Layout.AssignmentItemLayout, null);
                }

                view.SetBackgroundColor (Resources.GetColor (Resource.Color.assignmentblue));
                var number = view.FindViewById<TextView> (Resource.Id.assignmentItemNumber);
                var job = view.FindViewById<TextView> (Resource.Id.assignmentJob);
                var name = view.FindViewById<TextView> (Resource.Id.assignmentName);
                var phone = view.FindViewById<TextView> (Resource.Id.assignmentPhone);
                var address = view.FindViewById<TextView> (Resource.Id.assignmentAddress);
                var buttonLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentButtonLayout);
                var timerLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentTimerLayout);
                var timerlinearLayout = view.FindViewById<LinearLayout> (Resource.Id.timerLinearLayout);
                var spinner = view.FindViewById<Spinner> (Resource.Id.assignmentStatus);
                var spinnerImage = view.FindViewById<ImageView> (Resource.Id.assignmentStatusImage);
                var timer = view.FindViewById<ToggleButton> (Resource.Id.assignmentTimer);
                var timerText = view.FindViewById<TextView> (Resource.Id.assignmentTimerText);

                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;
                List<string> status = new List<string> ();
                foreach (var item in Enum.GetValues (typeof (AssignmentStatus))) {
                    status.Add (item.ToString ());
                }
                spinner.Adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, status);
                spinner.ItemSelected += (sender, e) => {
                    var selected = status.ElementAtOrDefault (e.Position);
                    if (selected != null) {
                        switch (selected) {
                            case "Active":
                                break;
                            default:
                                view.SetBackgroundColor (Resources.GetColor (Resource.Color.assignmentgrey));
                                spinnerImage.SetImageResource (Resource.Drawable.HoldImage);
                                break;
                        }
                    }
                };
                spinner.SetSelection (status.IndexOf (assignment.Status.ToString ()));
                number.Text = assignment.Priority.ToString ();
                job.Text = string.Format ("#{0} {1}\n{2}", assignment.JobNumber, assignment.StartDate.ToShortDateString (), assignment.Title);
                name.Text = assignment.ContactName;
                phone.Text = assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
                timerText.Text = string.Format ("{0} hr {1} min\n{2}", "10", "59", "START");

            } else {
                if (assignmentMapViewLayout.ChildCount > 0) {
                    assignmentMapViewLayout.RemoveViewAt (0);
                }
            }
        }
    }
}