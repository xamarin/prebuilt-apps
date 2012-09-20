using System;
using MonoTouch.UIKit;
using EmployeeDirectory.Views;
using EmployeeDirectory.Data;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using System.Collections.Generic;
using EmployeeDirectory.Utilities;

namespace EmployeeDirectory.iOS
{
	public class SearchViewController : UITableViewController
	{
		const string PlaceholderImagePath = "Images/Placeholder.jpg";

		readonly System.Threading.Tasks.TaskScheduler uiScheduler = 
			System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext ();

		SearchViewModel searchViewModel;

		SearchEditorView editor;

		public SearchViewController (IDirectoryService service, Search search)
		{
			searchViewModel = new SearchViewModel (service, search);

			searchViewModel.Error += (sender, e) => this.ShowError ("Search Error", e.Exception);
			searchViewModel.SearchCompleted += (sender, e) => TableView.ReloadData ();

			Title = searchViewModel.Title;

			editor = new SearchEditorView (searchViewModel.SearchText, searchViewModel.SearchProperty);
			editor.Search += (sender, e) => {
				searchViewModel.SearchText = editor.Text;
				searchViewModel.SearchProperty = editor.Property;
				searchViewModel.Search ();
			};

			TableView.TableHeaderView = editor;
			TableView.DataSource = new PeopleDataSource (this);
			TableView.Delegate = new PeopleDelegate (this);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//
			// Deselect all cells when appearing
			//
			var sels = TableView.IndexPathsForSelectedRows;
			if (sels != null) {
				foreach (var sel in sels) {
					TableView.DeselectRow (sel, true);
				}
			}
		}

		#region Image Support

		readonly Dictionary<string, UIImage> images = new Dictionary<string, UIImage> ();
		readonly List<string> imageDownloadsInProgress = new List<string> ();
		readonly ImageDownloader imageDownloader = new UIKitImageDownloader ();

		void LoadImagesForOnscreenRows ()
		{
			if (searchViewModel.GroupedPeople.Count > 0) {

				var visiblePaths = TableView.IndexPathsForVisibleRows;

				foreach (var indexPath in visiblePaths) {
					var person = ((PeopleDataSource)TableView.DataSource).GetPerson (indexPath);

					if (person.HasEmail) {
						if (!images.ContainsKey (person.Id)) {
							StartImageDownload (indexPath, person);
						}
					}
					else {
						FinishImageDownload (indexPath, person, UIImage.FromBundle (PlaceholderImagePath));
					}
				}
			}
		}

		void StartImageDownload (NSIndexPath indexPath, Person person)
		{
			if (imageDownloadsInProgress.Contains (person.Id)) return;
			imageDownloadsInProgress.Add (person.Id);

			imageDownloader.GetImageAsync (Gravatar.GetUrl (person.Email, 88)).ContinueWith (t => {
				if (!t.IsFaulted) {
					FinishImageDownload (indexPath, person, (UIImage)t.Result);
				}
			}, uiScheduler);
		}

		void FinishImageDownload (NSIndexPath indexPath, Person person, UIImage image)
		{
			images [person.Id] = image;
			imageDownloadsInProgress.Remove (person.Id);

			if (indexPath.Section < searchViewModel.GroupedPeople.Count &&
				indexPath.Row < searchViewModel.GroupedPeople [indexPath.Section].People.Count) {

				var cell = TableView.CellAt (indexPath);
				if (cell != null) {
					cell.ImageView.Image = image;
				}
			}
		}

		#endregion

		class PeopleDelegate : UITableViewDelegate
		{
			SearchViewController controller;

			public PeopleDelegate (SearchViewController controller)
			{
				this.controller = controller;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var person = ((PeopleDataSource)tableView.DataSource).GetPerson (indexPath);
				var vc = new PersonViewController (person);
				controller.NavigationController.PushViewController (vc, true);
			}

			[Export ("scrollViewDidEndDragging:willDecelerate:")]
			public void DidEndDragging (UIScrollView scrollView, bool willDecelerate)
			{
				if (!willDecelerate) {
					controller.LoadImagesForOnscreenRows ();
				}
			}

			[Export ("scrollViewDidEndDecelerating:")]
			public void DidEndDecelerating (UIScrollView scrollView)
			{
				controller.LoadImagesForOnscreenRows ();
			}
		}

		class PeopleDataSource : UITableViewDataSource
		{
			SearchViewController controller;
			public PeopleDataSource (SearchViewController controller)
			{
				this.controller = controller;
			}
			public override string TitleForHeader (UITableView tableView, int section)
			{
				return controller.searchViewModel.GroupedPeople[section].Title;
			}
			public override string[] SectionIndexTitles (UITableView tableView)
			{
				return controller.searchViewModel.GroupedPeople.Select (x => x.Title).ToArray ();
			}
			public override int NumberOfSections (UITableView tableView)
			{
				return controller.searchViewModel.GroupedPeople.Count;
			}
			public override int RowsInSection (UITableView tableView, int section)
			{
				return controller.searchViewModel.GroupedPeople[section].People.Count;
			}
			public Person GetPerson (NSIndexPath indexPath)
			{
				var personGroup = controller.searchViewModel.GroupedPeople[indexPath.Section];
				return personGroup.People[indexPath.Row];
			}
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell ("P") as PersonCell;
				if (cell == null) {
					cell = new PersonCell ("P");
				}

				var person = GetPerson (indexPath);
				cell.Person = person;

				UIImage image;
				if (controller.images.TryGetValue (person.Id, out image)) {
					cell.ImageView.Image = image;
				}
				else {
					if (!tableView.Dragging && !tableView.Decelerating && person.HasEmail) {
						controller.StartImageDownload (indexPath, person);
					}
					cell.ImageView.Image = UIImage.FromBundle (PlaceholderImagePath);
				}

				return cell;
			}
		}

		class PersonCell : UITableViewCell
		{
			static readonly UIFont NormalFont = UIFont.SystemFontOfSize (16);
			static readonly UIFont BoldFont = UIFont.BoldSystemFontOfSize (16);
			static readonly UIFont DetailsFont = UIFont.SystemFontOfSize (12);

			UILabel firstNameLabel;
			UILabel lastNameLabel;
			UILabel detailsLabel;

			Person person = null;
			public Person Person {
				get { return person; }
				set {
					if (person != value) {
						person = value;
						UpdateUI ();
					}
				}
			}

			public PersonCell (string id)
				: base (UITableViewCellStyle.Default, id)
			{
				firstNameLabel = new UILabel {
					Font = NormalFont,
				};
				ContentView.Add (firstNameLabel);

				lastNameLabel = new UILabel {
					Font = BoldFont,
				};
				ContentView.Add (lastNameLabel);

				detailsLabel = new UILabel {
					Font = DetailsFont,
					TextColor = UIColor.Gray,
				};
				ContentView.Add (detailsLabel);
			}

			void UpdateUI ()
			{
				var x = 44 + 6;

				var fn = person.FirstNameAndInitials;
				firstNameLabel.Text = fn;
				var fnw = string.IsNullOrEmpty (fn) ? 
						0.0f : 
						firstNameLabel.StringSize (fn, NormalFont).Width;
				var f = new RectangleF (x, 4, fnw + 4, 20);
				firstNameLabel.Frame = f;

				var ln = person.SafeLastName;
				lastNameLabel.Text = ln;
				var lnw = string.IsNullOrEmpty (ln) ?
						0.0f :
						lastNameLabel.StringSize (ln, BoldFont).Width;
				f.X = f.Right;
				f.Width = lnw;
				lastNameLabel.Frame = f;

				detailsLabel.Text = person.TitleAndDepartment ?? "";
				detailsLabel.Frame = new RectangleF (x, 25, 258, 14);
			}
		}
	}
}

