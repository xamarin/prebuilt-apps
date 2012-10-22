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
using FieldService.Android.Utilities;
using FieldService.Data;

namespace FieldService.Android.Fragments {
    public class NavigationFragment : Fragment, AdapterView.IOnItemClickListener {
        ListView navigationListView;
        Spinner navigationStatus;
        ImageView navigationStatusImage;
        TextView timerHours,
            timerMinutes;
        ToggleButton timer;
        RelativeLayout timerLayout;
        int lastposition;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.NavigationLayout, container, true);
            navigationListView = view.FindViewById<ListView> (Resource.Id.navigationListView);
            navigationStatus = view.FindViewById<Spinner> (Resource.Id.fragmentStatus);
            navigationStatusImage = view.FindViewById<ImageView> (Resource.Id.fragmentStatusImage);
            timerLayout = view.FindViewById<RelativeLayout> (Resource.Id.fragmentTimerTextLayout);
            navigationStatus.Adapter = new SpinnerAdapter (FieldService.Data.Assignment.AvailableStatuses, Activity);
            timer = view.FindViewById<ToggleButton> (Resource.Id.fragmentTimer);
            timerMinutes = view.FindViewById<TextView> (Resource.Id.fragmentMinutes);
            timerHours = view.FindViewById<TextView> (Resource.Id.fragmentHours);

            navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
            timerLayout.Visibility = ViewStates.Gone;

            var adapter = new NavigationAdapter (this.Activity, Resource.Layout.NavigationListItemLayout, Constants.Navigation);
            navigationListView.OnItemClickListener = this;
            navigationListView.Adapter = adapter;
            return view;
        }

        public Assignment Assignment
        {
            set
            {
                if (value != null && value.Status == AssignmentStatus.Active) {
                    timerLayout.Visibility = ViewStates.Visible;
                    navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
                    timerMinutes.Text = (value.TotalTicks / 60).ToString();
                    timerHours.Text = (value.TotalTicks / 60).ToString();
                } else {
                    navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
                    timerLayout.Visibility = ViewStates.Gone;
                }
            }
        }

        public void OnItemClick (AdapterView parent, View view, int position, long id)
        {
            if (position != lastposition) {

                if (navigationListView.FirstVisiblePosition <= lastposition && lastposition <= navigationListView.LastVisiblePosition) {
                    var oldView = navigationListView.GetChildAt (lastposition);
                    if (view != null) {
                        navigationListView.Adapter.GetView (lastposition, oldView, navigationListView);
                    }
                }
            }
            var image = view.FindViewById<ImageView> (Resource.Id.navigationListViewImage);
            image.Visibility = ViewStates.Visible;
            lastposition = position;
        }
    }
}