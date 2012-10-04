using System;
using System.Linq;
using System.Text;

namespace EmployeeDirectory
{
	public class Gravatar
	{
        static Func<byte[], byte[]> md5;

        static Gravatar ()
        {
#if SILVERLIGHT
            md5 = MD5Core.GetHash;
#else
            md5 = System.Security.Cryptography.MD5.Create ().ComputeHash;
#endif
        }

		public static Uri GetUrl (string email, int size)
		{
			if (string.IsNullOrEmpty (email)) {
				throw new ArgumentException ("Email must be a valid email address.", "email");
			}
			if (size <= 0) {
				throw new ArgumentException ("Size must be greater than 0.", "size");
			}

			var hash = md5 (Encoding.UTF8.GetBytes (email));
			var hashString = string.Join ("", hash.Select (x => x.ToString ("x2")));
			return new Uri ("http://www.gravatar.com/avatar/" + hashString + ".jpg?s=" + size + "&d=mm");
		}
	}
}

