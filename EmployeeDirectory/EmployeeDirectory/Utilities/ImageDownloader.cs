using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace EmployeeDirectory.Utilities
{
	public abstract class ImageDownloader
	{
		IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication ();

		TimeSpan cacheDuration;

		public ImageDownloader ()
			: this (TimeSpan.FromDays (1))
		{
		}

		public ImageDownloader (TimeSpan cacheDuration)
		{
			this.cacheDuration = cacheDuration;

			if (!store.DirectoryExists ("ImageCache")) {
				store.CreateDirectory ("ImageCache");
			}
		}

		public Task<object> GetImageAsync (Uri uri)
		{
			return Task.Factory.StartNew (() => {

				var now = DateTime.UtcNow;

				var filename = Uri.EscapeDataString (uri.AbsoluteUri);

				var lastWriteTime = GetLastWriteTimeUtc (filename);

				if (lastWriteTime.HasValue && (now - lastWriteTime.Value) < cacheDuration) {
					using (var o = OpenStorage (filename, FileMode.Open)) {
						return LoadImage (o);
					}
				}
				else {
					using (var d = DownloadAsync (uri).Result) {
						using (var o = OpenStorage (filename, FileMode.Create)) {
							d.CopyTo (o);
						}
					}
					using (var o = OpenStorage (filename, FileMode.Open)) {
						return LoadImage (o);
					}
				}
			});
		}

		Task<Stream> DownloadAsync (Uri uri)
		{
			System.Diagnostics.Debug.WriteLine ("Downloading image: " + uri);

			var req = WebRequest.Create (uri);

			var getTask = Task.Factory.FromAsync<WebResponse> (
				req.BeginGetResponse, req.EndGetResponse, null);

			return getTask.ContinueWith (task => {
				if (task.IsFaulted) {
					Console.WriteLine ("FAILED " + uri);
				}
				var res = task.Result;
				System.Diagnostics.Debug.WriteLine ("Downloaded image: " + uri);
				return res.GetResponseStream ();
			});
		}

		protected virtual DateTime? GetLastWriteTimeUtc (string fileName)
		{
			var path = Path.Combine ("ImageCache", fileName);
			if (store.FileExists (path)) {
				return store.GetLastWriteTime (path).UtcDateTime;
			} else {
				return null;
			}
		}

		protected virtual Stream OpenStorage (string fileName, FileMode mode)
		{
			return store.OpenFile (Path.Combine ("ImageCache", fileName), mode);
		}

		protected abstract object LoadImage (Stream stream);
	}
}

