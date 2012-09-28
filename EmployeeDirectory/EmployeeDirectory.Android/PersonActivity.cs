
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
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;
using System.IO;

namespace EmployeeDirectory.Android
{
	[Activity (Label = "Person", Theme = "@android:style/Theme.Holo.Light")]			
	public class PersonActivity : Activity
	{
		PersonViewModel personViewModel;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//
			// Get the person object from the intent
			//
			Person person;
			if (Intent.HasExtra ("Person")) {
				var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ();
				var personBytes = Intent.GetByteArrayExtra ("Person");
				person = (Person)formatter.Deserialize (new MemoryStream (personBytes));
			}
			else {
				person = new Person ();
			}

			//
			// Load the View Model
			//
			personViewModel = new PersonViewModel (person);

			Title = person.SafeDisplayName;
		}

		/// <summary>
		/// Creates the intent that can be used to present this activity given
		/// a specific Person object.
		/// </summary>
		/// <returns>
		/// The intent.
		/// </returns>
		/// <param name='person'>
		/// The Person to show in this activity.
		/// </param>
		public static Intent CreateIntent (Context context, Person person)
		{
			var intent = new Intent (context, typeof (PersonActivity));
			var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ();
			var personStream = new MemoryStream ();
			formatter.Serialize (personStream, person);
			intent.PutExtra ("Person", personStream.ToArray ());
			return intent;
		}
	}
}

