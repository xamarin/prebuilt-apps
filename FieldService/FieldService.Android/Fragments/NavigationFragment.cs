using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    public class NavigationFragment : Fragment, AdapterView.IOnItemClickListener, AdapterView.IOnItemSelectedListener {
        ListView navigationListView;
        Spinner navigationStatus;
        ImageView navigationStatusImage;
        TextView timerHours,
            timerMinutes;
        ToggleButton timer;
        RelativeLayout timerLayout;
        int lastposition,
            listViewIndex;
        Assignment assignment;
        AssignmentViewModel assignmentViewModel;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            if (savedInstanceState != null && savedInstanceState.ContainsKey (Constants.BUNDLE_INDEX)) {
                listViewIndex = savedInstanceState.GetInt (Constants.BUNDLE_INDEX);
            }
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.NavigationLayout, container, true);
            navigationListView = view.FindViewById<ListView> (Resource.Id.navigationListView);
            navigationStatus = view.FindViewById<Spinner> (Resource.Id.fragmentStatus);
            navigationStatusImage = view.FindViewById<ImageView> (Resource.Id.fragmentStatusImage);
            timerLayout = view.FindViewById<RelativeLayout> (Resource.Id.fragmentTimerTextLayout);
            timer = view.FindViewById<ToggleButton> (Resource.Id.fragmentTimer);
            timerMinutes = view.FindViewById<TextView> (Resource.Id.fragmentMinutes);
            timerHours = view.FindViewById<TextView> (Resource.Id.fragmentHours);

            navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
            navigationStatus.Adapter = new SpinnerAdapter (Assignment.AvailableStatuses, Activity);
            navigationStatus.OnItemSelectedListener = this;
            timerLayout.Visibility = ViewStates.Gone;

            var adapter = new NavigationAdapter (this.Activity, Resource.Layout.NavigationListItemLayout, Constants.Navigation);
            navigationListView.OnItemClickListener = this;
            navigationListView.Adapter = adapter;
            var navItemView = navigationListView.GetChildAt (listViewIndex);
            if (navItemView != null)
            {
                OnItemClick (navigationListView, navItemView, listViewIndex, 0);
            }
            return view;
        }

        private void SaveAssignment ()
        {
            assignmentViewModel.SaveAssignment (assignment).ContinueOnUIThread (_ => {
                var activity = (SummaryActivity)Activity;
                activity.ReloadNavigation ();
                });
        }

        public Assignment Assignment
        {
            get { return assignment; }
            set
            {
                if (value != null && value.Status == AssignmentStatus.Active) {
                    timerLayout.Visibility = ViewStates.Visible;
                    navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
                    navigationStatus.SetSelection (Assignment.AvailableStatuses.ToList ().IndexOf (AssignmentStatus.Active));
                    timerMinutes.Text = assignmentViewModel.Hours.ToString ("hh");
                    timerHours.Text = assignmentViewModel.Hours.ToString ("mm");
                } else {
                    navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
                    timerLayout.Visibility = ViewStates.Gone;
                }
                assignment = value;
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

        public void OnItemSelected (AdapterView parent, View view, int position, long id)
        {
            var status = Assignment.AvailableStatuses[position];
            if (Assignment.Status != status) {
                assignment.Status = status;
                SaveAssignment ();
            }
        }

        public void OnNothingSelected (AdapterView parent)
        {
            //do nothing
        }

        public override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState (outState);
            outState.PutInt (Constants.BUNDLE_INDEX, lastposition);
        }
    }
}