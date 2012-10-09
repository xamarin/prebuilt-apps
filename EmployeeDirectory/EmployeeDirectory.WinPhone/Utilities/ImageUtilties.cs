using System;
using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Linq;

namespace EmployeeDirectory.WinPhone.Utilities
{
    public static class ImageUtilties
    {
		const string SharedDirectory = "Shared";
		const string SharedShellContentDirectory = SharedDirectory + "\\ShellContent";

		static ImageUtilties ()
		{
			var iso = IsolatedStorageFile.GetUserStoreForApplication ();
			if (!iso.DirectoryExists (SharedDirectory)) {
				iso.CreateDirectory (SharedDirectory);
			}

			if (!iso.DirectoryExists (SharedShellContentDirectory)) {
				iso.CreateDirectory (SharedShellContentDirectory);
			}
		}

        public static Uri SaveAsTile (this BitmapSource image, string key)
        {
			var safeKey = new string (key.Where (c => char.IsLetterOrDigit (c)).ToArray ());            

			var bmp = new WriteableBitmap (image);

			var path = SharedShellContentDirectory + "\\" + safeKey + ".jpg";

			var iso = IsolatedStorageFile.GetUserStoreForApplication ();
            using (var stream = iso.CreateFile (path)) {
                bmp.SaveJpeg (stream, 173, 173, 0, 90);
            }

			return new Uri ("isostore:/Shared/ShellContent/" + safeKey + ".jpg", UriKind.RelativeOrAbsolute);
        }
    }
}
