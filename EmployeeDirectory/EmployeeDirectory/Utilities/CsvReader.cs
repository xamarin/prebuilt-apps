using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EmployeeDirectory
{
	public class CsvReader<T>
		where T : new ()
	{
		TextReader reader;

		string[] headerNames;

		public CsvReader (TextReader reader)
		{
			this.reader = reader;
		}

		public IEnumerable<T> ReadAll ()
		{
			//
			// Associate header names with properties
			//
			headerNames = reader.ReadLine ().Split (',');

			var props = new PropertyInfo[headerNames.Length];
			for (var hi = 0; hi < props.Length; hi++) {
				var p = typeof(T).GetProperty (headerNames[hi]);
				if (p == null) throw new ApplicationException (
					"Property '" + headerNames[hi] + "' not found in " + typeof (T).Name);
				props[hi] = p;
			}

			//
			// Read all the records
			//
			var r = new T ();
			var i = 0;

			var ch = reader.Read ();
			while (ch > 0) {
				if (ch == '\n') {
					yield return r;
					r = new T ();
					i = 0;
					ch = reader.Read ();
				}
				else if (ch == '\r') {
					ch = reader.Read ();
				}
				else if (ch == '"') {
					ch = ReadQuoted (r, props[i]);
				}
				else if (ch == ',') {
					i++;
					ch = reader.Read ();
				}
				else {
					ch = ReadNonQuoted (r, props[i], (char)ch);
				}
			}
		}

		int ReadNonQuoted (T r, PropertyInfo prop, char first)
		{
			var sb = new StringBuilder ();

			sb.Append (first);

			var ch = reader.Read ();

			while (ch >= 0 && ch != ',' && ch != '\r' && ch != '\n') {
				sb.Append ((char)ch);
				ch = reader.Read ();
			}

			prop.SetValue (r, Convert.ChangeType (sb.ToString ().Trim (), prop.PropertyType), null);

			return ch;
		}

		int ReadQuoted (T r, PropertyInfo prop)
		{
			var sb = new StringBuilder ();

			var ch = reader.Read ();

			var hasQuote = false;

			while (ch >= 0) {

				if (ch == '"') {
					if (hasQuote) {
						sb.Append ('"');
						hasQuote = false;
					}
					else {
						hasQuote = true;
					}
				}
				else {
					if (hasQuote) {
						prop.SetValue (r, Convert.ChangeType (sb.ToString ().Trim (), prop.PropertyType), null);
						return ch;
					}
					else {
						sb.Append ((char)ch);
					}
				}

				ch = reader.Read ();
			}

			prop.SetValue (r, Convert.ChangeType (sb.ToString ().Trim (), prop.PropertyType), null);
			return ch;
		}
	}
}

