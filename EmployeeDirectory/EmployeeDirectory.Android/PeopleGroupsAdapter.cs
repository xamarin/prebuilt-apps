
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
using EmployeeDirectory.ViewModels;
using System.Collections.ObjectModel;
using EmployeeDirectory.Data;

namespace EmployeeDirectory.Android
{
	public class PeopleGroupsAdapter : BaseAdapter
	{
		Activity context;

		ICollection<PeopleGroup> itemsSource = new ObservableCollection<PeopleGroup> ();

		List<Java.Lang.Object> items = new List<Java.Lang.Object> ();

		public ICollection<PeopleGroup> ItemsSource
		{
			get {
				return itemsSource;
			}
			set {
				if (itemsSource != value && value != null) {
					itemsSource = value;

					items.Clear ();
					foreach (var g in itemsSource) {
						items.Add (new GroupItem (g));
						foreach (var p in g.People) {
							items.Add (new PersonItem (p));
						}
					}

					this.NotifyDataSetChanged ();
				}
			}
		}

		public PeopleGroupsAdapter (Activity context)
		{
			this.context = context;
		}

		public override int ViewTypeCount {
			get {
				return 2;
			}
		}

		public override int GetItemViewType (int position)
		{
			return items[position] is GroupItem ? 0 : 1;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return items[position];
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			var personItem = item as PersonItem;

			if (personItem != null) {

				var v = convertView;
				if (v == null) {
					v = context.LayoutInflater.Inflate (Resource.Layout.PersonListItem, null);
				}
				var nameTextView = v.FindViewById<TextView> (Resource.Id.NameTextView);
				var detailsTextView = v.FindViewById<TextView> (Resource.Id.DetailsTextView);

				nameTextView.Text = personItem.Person.SafeDisplayName;
				detailsTextView.Text = personItem.Person.TitleAndDepartment;

				return v;
			}
			else {
				var v = convertView as TextView;
				if (v == null) {
					v = new TextView (context);
				}
				v.Text = items[position].ToString ();
				return v;
			}
		}

		public override int Count {
			get {
				return items.Count;
			}
		}

		class GroupItem : Java.Lang.Object
		{
			public PeopleGroup Group { get; private set; }
			
			public GroupItem (PeopleGroup group)
			{
				Group = group;
			}

			public override string ToString ()
			{
				return Group.Title;
			}
		}

		class PersonItem : Java.Lang.Object
		{
			public Person Person { get; private set; }

			public PersonItem (Person person)
			{
				Person = person;
			}

			public override string ToString ()
			{
				return Person.SafeDisplayName;
			}
		}
	}
}

