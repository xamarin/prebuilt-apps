
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EmployeeDirectory.Utilities;
using Android.Graphics;

namespace EmployeeDirectory.Android
{
	public class AndroidImageDownloader : ImageDownloader
	{
		public AndroidImageDownloader ()
			: base (2)
		{
		}

		protected override object LoadImage (System.IO.Stream stream)
		{
			return BitmapFactory.DecodeStream (stream);
		}
	}
}

