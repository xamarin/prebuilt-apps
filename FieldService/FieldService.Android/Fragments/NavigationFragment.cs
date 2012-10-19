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
using FieldService.Data;

namespace FieldService.Android.Fragments {
    public class NavigationFragment : Fragment {
        ListView navigationListView;
        Spinner navigationStatus;
        ImageView navigationStatusImage;
        TextView timerHours,
            timerMinutes;
        ToggleButton timer;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.NavigationLayout, container, true);
            navigationListView = view.FindViewById<ListView> (Resource.Id.navigationListView);
            navigationStatus = view.FindViewById<Spinner> (Resource.Id.fragmentStatus);
            navigationStatusImage = view.FindViewById<ImageView> (Resource.Id.fragmentStatusImage);
            var layout = view.FindViewById<RelativeLayout> (Resource.Id.fragmentTimerTextLayout);
            navigationStatus.Adapter = new SpinnerAdapter (FieldService.Data.Assignment.AvailableStatuses, Activity);

            if (Assignment != null && Assignment.Status == AssignmentStatus.Active) {
                timer = view.FindViewById<ToggleButton> (Resource.Id.fragmentTimer);
                timerMinutes = view.FindViewById<TextView> (Resource.Id.fragmentMinutes);
                timerHours = view.FindViewById<TextView> (Resource.Id.fragmentHours);
                layout.Visibility = ViewStates.Visible;
                navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
            } else {
                navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
                layout.Visibility = ViewStates.Gone;
            }

            return view;
        }

        public Assignment Assignment
        {
            get;
            set;
        }
    }
}