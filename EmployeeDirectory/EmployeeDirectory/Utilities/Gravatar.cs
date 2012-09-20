using System;
using System.Linq;
using System.Text;

namespace EmployeeDirectory
{
	public class Gravatar
	{
		static System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create ();
		
		public static Uri GetUrl (string email, int size)
		{
			if (string.IsNullOrEmpty (email)) {
				throw new ArgumentException ("Email must be a valid email address.", "email");
			}
			if (size <= 0) {
				throw new ArgumentException ("Size must be greater than 0.", "size");
			}

			var hash = md5.ComputeHash (Encoding.UTF8.GetBytes (email));
			var hashString = string.Join ("", hash.Select (x => x.ToString ("x2")));
			return new Uri ("http://www.gravatar.com/avatar/" + hashString + ".jpg?s=" + size + "&d=mm");
		}
	}
}

