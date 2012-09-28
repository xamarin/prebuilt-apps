using System;
using EmployeeDirectory.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.ViewModels
{
	public class PersonViewModel : ViewModel
	{
		readonly IFavoritesRepository favoritesRepository;

		public PersonViewModel (Person person, IFavoritesRepository favoritesRepository)
		{
			if (person == null) {
				throw new ArgumentNullException ("person");
			}
			if (favoritesRepository == null) {
				throw new ArgumentNullException ("favoritesRepository");
			}

			Person = person;
			this.favoritesRepository = favoritesRepository;

			PropertyGroups = new ObservableCollection<PropertyGroup> ();

			var general = new PropertyGroup ("General");
			general.Add ("Name", person.SafeDisplayName, PropertyType.Generic);
			general.Add ("Title", person.Title, PropertyType.Generic);
			general.Add ("Department", person.Department, PropertyType.Generic);
			general.Add ("Company", person.Company, PropertyType.Generic);
			general.Add ("Manager", person.Manager, PropertyType.Generic);
			general.Add ("Description", person.Description, PropertyType.Generic);
			if (general.Properties.Count > 0) {
				PropertyGroups.Add (general);
			}

			var phone = new PropertyGroup ("Phone");
			foreach (var p in person.TelephoneNumbers) {
				phone.Add ("Phone", p, PropertyType.Phone);
			}
			foreach (var p in person.HomeNumbers) {
				phone.Add ("Home", p, PropertyType.Phone);
			}
			foreach (var p in person.MobileNumbers) {
				phone.Add ("Mobile", p, PropertyType.Phone);
			}
			if (phone.Properties.Count > 0) {
				PropertyGroups.Add (phone);
			}

			var online = new PropertyGroup ("Online");
			online.Add ("Email", person.Email, PropertyType.Email);
			online.Add ("WebPage", person.WebPage, PropertyType.Url);			
			online.Add ("Twitter", person.Twitter, PropertyType.Twitter);
			if (online.Properties.Count > 0) {
				PropertyGroups.Add (online);
			}

			var address = new PropertyGroup ("Address");
			address.Add ("Office", person.Office, PropertyType.Generic);
			address.Add ("Address", AddressString, PropertyType.Address);
			if (address.Properties.Count > 0) {
				PropertyGroups.Add (address);
			}
		}

		#region View Data

		public Person Person { get; private set; }

		public ObservableCollection<PropertyGroup> PropertyGroups { get; private set; }

		public bool IsFavorite {
			get { return favoritesRepository.IsFavorite (Person); }
		}

		string AddressString {
			get {
				var sb = new StringBuilder ();
				if (!string.IsNullOrWhiteSpace (Person.Street)) {
					sb.AppendLine (Person.Street.Trim ());
				}
				if (!string.IsNullOrWhiteSpace (Person.POBox)) {
					sb.AppendLine (Person.POBox.Trim ());
				}
				if (!string.IsNullOrWhiteSpace (Person.City)) {
					sb.AppendLine (Person.City.Trim ());
				}
				if (!string.IsNullOrWhiteSpace (Person.State)) {
					sb.AppendLine (Person.State.Trim ());
				}
				if (!string.IsNullOrWhiteSpace (Person.PostalCode)) {
					sb.AppendLine (Person.PostalCode.Trim ());
				}
				if (!string.IsNullOrWhiteSpace (Person.Country)) {
					sb.AppendLine (Person.Country.Trim ());
				}
				return sb.ToString ();
			}
		}

		public class PropertyGroup
		{
			public string Name { get; private set; }
			public ObservableCollection<PropertyValue> Properties { get; private set; }

			public PropertyGroup (string name)
			{
				Name = name;
				Properties = new ObservableCollection<PropertyValue> ();
			}

			public void Add (string name, string value, PropertyType type)
			{
				if (!string.IsNullOrWhiteSpace (value)) {
					Properties.Add (new PropertyValue (name, value, type));
				}
			}
		}

		public class PropertyValue
		{
			public string Name { get; private set; }
			public string Value { get; private set; }
			public PropertyType Type { get; private set; }

			public PropertyValue (string name, string value, PropertyType type)
			{
				Name = name;
				Value = value.Trim ();
				Type = type;
			}

			public override string ToString ()
			{
				return string.Format ("{0} = {1}", Name, Value);
			}
		}

		public enum PropertyType
		{
			Generic,
			Phone,
			Email,
			Url,
			Twitter,
			Address,
		}

		#endregion

		#region Commands

		public void ToggleFavorite ()
		{			
			if (favoritesRepository.IsFavorite (Person)) {
				favoritesRepository.Delete (Person);
			}
			else {
				favoritesRepository.InsertOrUpdate (Person);
			}
			OnPropertyChanged ("IsFavorite");
		}

		#endregion
	}
}

