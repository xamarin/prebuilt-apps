using System;
using EmployeeDirectory.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeeDirectory.ViewModels
{
	public class PeopleGroup
	{
		public string Title { get; private set; }
		public List<Person> People { get; private set; }
		public PeopleGroup (string title)
		{
			Title = title;
			People = new List<Person> ();
		}

		public static ObservableCollection<PeopleGroup> CreateGroups (IEnumerable<Person> people)
		{
			var pgs = new Dictionary<string, PeopleGroup> ();

			foreach (var p in people.OrderBy (x => x.LastName)) {
				
				var g = p.SafeLastName.Substring (0, 1).ToUpperInvariant ();
				
				PeopleGroup pg;
				if (!pgs.TryGetValue (g, out pg)) {
					pg = new PeopleGroup (g);
					pgs.Add (g, pg);
				}
				
				pg.People.Add (p);
			}
			
			return new ObservableCollection<PeopleGroup> (pgs.Values.OrderBy (x => x.Title));
		}
	}
}

