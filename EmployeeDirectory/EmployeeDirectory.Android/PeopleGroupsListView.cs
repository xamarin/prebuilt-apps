
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EmployeeDirectory.Utilities;
using Android.Graphics;

namespace EmployeeDirectory.Android
{
	public class PeopleGroupsListView : ListView
	{
		public ScrollState ScrollState { get; private set; }

		public PeopleGroupsListView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public PeopleGroupsListView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			ScrollState = ScrollState.Idle;
			ScrollStateChanged += HandleScrollStateChanged;
		}

		void HandleScrollStateChanged (object sender, ScrollStateChangedEventArgs e)
		{
			ScrollState = e.ScrollState;

			if (e.ScrollState == ScrollState.Idle) {
				((PeopleGroupsAdapter)Adapter).LoadImagesForOnscreenRows (this);
			}
		}
	}
}

