using System;
using System.IO;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EmployeeDirectory.Data
{
	/// <summary>
	/// Represents a search: a filter and the latest results.
	/// </summary>
	public class Search
	{
		string name;
		public string Name {
			get { lock (mutex) return name; }
			set { lock (mutex) name = value; }
		}

		string text;
		public string Text {
			get { lock (mutex) return text; }
			set { lock (mutex) text = value; }
		}

		SearchProperty property;
		public SearchProperty Property {
			get { lock (mutex) return property; }
			set { lock (mutex) property = value; }
		}

		public Filter Filter {
			get {
				lock (mutex) {
					if (property == SearchProperty.All) {
						return new OrFilter (
							new ContainsFilter ("Name", text),
							new ContainsFilter ("Title", text),
							new ContainsFilter ("Department", text));
					}
					else {
						var propName = property.ToString ();
						return new ContainsFilter (propName, text);
					}
				}
			}
		}

		List<Person> results;
		public List<Person> Results {
			get { lock (mutex) return results; }
			set { lock (mutex) results = value; }
		}

		/// <summary>
		/// A key used to lock this object to makes reads and writes
		/// mutually exclusive. This is necessary so that we can save
		/// this object asynchronously.
		/// </summary>
		readonly object mutex = new object ();

		public Search ()
			: this ("")
		{
		}

		public Search (string name)
		{
			this.name = name;
			this.text = "";
			this.property = SearchProperty.Name;
			this.results = new List<Person> ();
		}

		public static Task<Search> OpenAsync (string name)
		{
			return Task.Factory.StartNew (() => {
				var store = IsolatedStorageFile.GetUserStoreForApplication ();
                var serializer = new XmlSerializer (typeof (Search));
				using (var stream = store.OpenFile (name + ".search", FileMode.Open)) {
					var s = (Search)serializer.Deserialize (stream);
					s.Name = name;
					return s;
				}
			});
		}

		public Task SaveAsync ()
		{
			if (string.IsNullOrEmpty (Name)) throw new InvalidOperationException ("Name must be set.");

			return Task.Factory.StartNew (() => {
				var store = IsolatedStorageFile.GetUserStoreForApplication ();
                var serializer = new XmlSerializer (typeof (Search));
				using (var stream = store.OpenFile (Name + ".search", FileMode.Create)) {
					lock (mutex) serializer.Serialize (stream, this);
				}
			});
		}
	}
}

