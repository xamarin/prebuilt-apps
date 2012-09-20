using System;
using EmployeeDirectory.Utilities;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;

namespace EmployeeDirectory.iOS
{
	/// <summary>
	/// A specialized ImageDownloader that uses the Library/Caches directory
	/// of iOS and that loads UIImages from files.
	/// </summary>
	public class UIKitImageDownloader : ImageDownloader
	{
		string cachePath;

		public UIKitImageDownloader ()
		{
			cachePath = Environment.GetFolderPath (Environment.SpecialFolder.InternetCache);
		}

		protected override DateTime? GetLastWriteTimeUtc (string fileName)
		{
			return File.GetLastWriteTimeUtc (Path.Combine (cachePath, fileName));
		}

		protected override Stream OpenStorage (string fileName, FileMode mode)
		{
			return File.Open (Path.Combine (cachePath, fileName), mode);
		}

		protected override object LoadImage (Stream stream)
		{
			return UIImage.LoadFromData (NSData.FromStream (stream));
		}
	}
}

