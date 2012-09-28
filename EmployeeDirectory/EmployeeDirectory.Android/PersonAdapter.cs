
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

namespace EmployeeDirectory.Android
{
	public class PersonAdapter : BaseAdapter
	{
		PersonViewModel viewModel;

		List<Item> items;

		public PersonAdapter (PersonViewModel viewModel)
		{
			items = new List<Item> ();

			foreach (var pg in viewModel.PropertyGroups) {
				items.Add (new MainHeaderItem (pg.Name));
				foreach (var p in pg.Properties) {
					items.Add (new PropertyItem (p));
				}
			}
		}

		public override int ViewTypeCount {
			get {
				return 3;
			}
		}

		public override int GetItemViewType (int position)
		{
			return items[position].ViewType;
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
			return items[position].GetView (convertView, parent);
		}

		public override int Count {
			get {
				return items.Count;
			}
		}

		#region Item types

		abstract class Item : Java.Lang.Object
		{
			public int ViewType { get; private set; }

			public Item (int viewType)
			{
				ViewType = viewType;
			}

			public abstract View GetView (View convertView, ViewGroup parent);
		}

		class ImageItem : Item
		{
			Uri url;

			public ImageItem (Uri url)
				: base (0)
			{
				this.url = url;
			}

			public override View GetView (View convertView, ViewGroup parent)
			{
				var v = convertView as ImageView;
				if (v == null) {
					v = new ImageView (parent.Context);
				}
				return v;
			}
		}

		class MainHeaderItem : Item
		{
			string text;

			public MainHeaderItem (string text)
				: base (1)
			{
				this.text = text;
			}

			public override View GetView (View convertView, ViewGroup parent)
			{
				var v = convertView;
				if (v == null) {
					var inflater = ((Activity)parent.Context).LayoutInflater;
					v = inflater.Inflate (Resource.Layout.GroupHeaderListItem, null);
				}

				var headerTextView = v.FindViewById<TextView> (Resource.Id.HeaderTextView);
				headerTextView.Text = text;

				return v;
			}
		}

		class PropertyItem : Item
		{
			PersonViewModel.PropertyValue property;

			public PropertyItem (PersonViewModel.PropertyValue property)
				: base (2)
			{
				this.property = property;
			}

			public override View GetView (View convertView, ViewGroup parent)
			{
				var v = convertView;
				if (v == null) {
					var inflater = ((Activity)parent.Context).LayoutInflater;
					v = inflater.Inflate (Resource.Layout.PropertyListItem, null);
				}

				var nameTextView = v.FindViewById<TextView> (Resource.Id.NameTextView);
				var valueTextView = v.FindViewById<TextView> (Resource.Id.ValueTextView);

				nameTextView.Text = property.Name;
				valueTextView.Text = property.Value;

				return v;
			}
		}

		#endregion
	}
}

