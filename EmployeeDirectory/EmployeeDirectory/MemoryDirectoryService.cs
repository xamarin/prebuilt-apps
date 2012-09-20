using System;
using System.Collections.Generic;
using EmployeeDirectory.Data;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EmployeeDirectory
{
	public class MemoryDirectoryService : IDirectoryService
	{
		List<Person> people;

		Dictionary<string, PropertyInfo> properties;

		public MemoryDirectoryService (IEnumerable<Person> people)
		{
			this.people = people.ToList ();
			this.properties = typeof (Person).GetProperties ().ToDictionary (p => p.Name);
		}

		#region IDirectoryService implementation

		public Task<IList<Person>> SearchAsync (Filter filter, int sizeLimit)
		{
			return Task.Factory.StartNew (() => {
				var s = Search (filter);
				var list = s.ToList ();
				return (IList<Person>)list;
			});
		}

		IEnumerable<Person> Search (Filter filter)
		{
			if (filter is OrFilter) {
				var f = (OrFilter)filter;
				var r = Enumerable.Empty<Person> ();
				foreach (var sf in f.Filters) {
					r = r.Concat (Search (sf));
				}
				return r.Distinct ();
			}
			else if (filter is AndFilter) {
				throw new NotImplementedException ();
			}
			else if (filter is NotFilter) {
				throw new NotImplementedException ();
			}
			else if (filter is EqualsFilter) {
				var f = (EqualsFilter)filter;
				var upper = f.Value.ToUpperInvariant ();
				var prop = properties[f.PropertyName];
				var q = from p in people
						let v = prop.GetValue (p, null)
						where v != null && v.ToString ().ToUpperInvariant () == upper
						select p;
				return q;
			}
			else if (filter is ContainsFilter) {
				var f = (ContainsFilter)filter;
				var re = new Regex (f.Value, RegexOptions.IgnoreCase);
				var prop = properties[f.PropertyName];
				var q = from p in people
						let v = prop.GetValue (p, null)
						where v != null && re.IsMatch (v.ToString ())
						select p;
				return q;
			}
			else {
				throw new NotSupportedException ("Unsupported filter type: " + filter.GetType ());
			}
		}

		#endregion

		#region File IO

		public static MemoryDirectoryService FromCsv (string path)
		{
			using (var reader = File.OpenText (path)) {
				return FromCsv (reader);
			}
		}

		public static MemoryDirectoryService FromCsv (TextReader textReader)
		{
			var reader = new CsvReader<Person> (textReader);
			return new MemoryDirectoryService (reader.ReadAll ());
		}

		#endregion
	}
}

