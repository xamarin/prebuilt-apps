using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.IO.IsolatedStorage;
using System.Xml;

namespace EmployeeDirectory.Data
{
	[XmlRoot ("Favorites")]
	public class XmlFavoritesRepository : IFavoritesRepository
	{
		string fileName;

		public List<Person> People { get; set; }

		public static XmlFavoritesRepository Open (string fileName)
		{
			var serializer = new XmlSerializer (typeof(XmlFavoritesRepository));
			var iso = IsolatedStorageFile.GetUserStoreForApplication ();

			try {
				using (var f = iso.OpenFile (fileName, FileMode.Open)) {
					var repo = (XmlFavoritesRepository)serializer.Deserialize (f);
					repo.fileName = fileName;
					return repo;
				}
			} catch (Exception) {
				return new XmlFavoritesRepository {
					fileName = fileName,
					People = new List<Person> ()
				};
			}
		}

		void Save ()
		{
			var serializer = new XmlSerializer (typeof(XmlFavoritesRepository));
			var iso = IsolatedStorageFile.GetUserStoreForApplication ();

			using (var f = iso.OpenFile (fileName, FileMode.Create)) {
				serializer.Serialize (f, this);
			}
		}

		#region IFavoritesRepository implementation

		public IEnumerable<Person> GetAll ()
		{
			return People;
		}

		public bool IsFavorite (Person person)
		{
			return People.Any (x => x.Id == person.Id);
		}

		public void InsertOrUpdate (Person person)
		{
			var existing = People.FirstOrDefault (x => x.Id == person.Id);
			if (existing != null) {
				People.Remove (existing);
			}
			People.Add (person);
			Save ();
		}

		public void Delete (Person person)
		{
			var newPeopleQ = from p in People where p.Id != person.Id select p;
			var newPeople = newPeopleQ.ToList ();
			var n = People.Count - newPeople.Count;
			People = newPeople;
			if (n != 0) {
				Save ();
			}
		}

		#endregion
	}
}

