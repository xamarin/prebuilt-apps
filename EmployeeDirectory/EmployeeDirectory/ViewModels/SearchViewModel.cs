using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeDirectory.Data;
using System.Linq;
using System.Threading;

namespace EmployeeDirectory.Views
{
	public class SearchViewModel
	{
		IDirectoryService service;
		Search search;

		public SearchViewModel (IDirectoryService service, Search search)
		{
			if (service == null) throw new ArgumentNullException ("service");
			this.service = service;

			if (search == null) throw new ArgumentNullException ("search");
			this.search = search;

			SetGroupedPeople ();
		}

		#region View Data

		public string Title {
			get {
				if (string.IsNullOrEmpty (search.Name)) {
					return "Search";
				}
				else {
					return search.Name;
				}
			}
		}

		public class PeopleGroup
		{
			public string Title { get; private set; }
			public List<Person> People { get; private set; }
			public PeopleGroup (string title)
			{
				Title = title;
				People = new List<Person> ();
			}
		}

		public List<PeopleGroup> GroupedPeople { get; private set; }

		public SearchProperty SearchProperty {
			get { return search.Property; }
			set { search.Property = value; }
		}

		public string SearchText {
			get { return search.Text; }
			set { search.Text = value.Trim (); }
		}

		#endregion


		#region Commands

		CancellationTokenSource lastCancelSource = null;

		public void Search ()
		{
			//
			// Stop previous search
			//
			if (lastCancelSource != null) {
				lastCancelSource.Cancel ();
			}

			//
			// Perform the search
			//
			var tokenSource = new CancellationTokenSource ();
			var token = tokenSource.Token;
			service.SearchAsync (search.Filter, 200).ContinueWith (searchTask => {

					if (searchTask.IsFaulted) {
						var ev = Error;
						if (ev != null) {
							ev (this, new ErrorEventArgs (searchTask.Exception));
						}
					}
					else {
						search.Results = searchTask.Result.ToList ();
						SetGroupedPeople ();
						search.SaveAsync ();

						var ev = SearchCompleted;
						if (ev != null && !token.IsCancellationRequested) {
							ev (this, EventArgs.Empty);
						}
					}

				},
				token,
				TaskContinuationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext ());

			lastCancelSource = tokenSource;
		}

		/// <summary>
		/// Groups people by the initial letter of their last name
		/// </summary>
		void SetGroupedPeople ()
		{
			var pgs = new Dictionary<string, PeopleGroup> ();
			foreach (var p in search.Results.OrderBy (x => x.LastName)) {

				var g = p.SafeLastName.Substring (0, 1).ToUpperInvariant ();

				PeopleGroup pg;
				if (!pgs.TryGetValue (g, out pg)) {
					pg = new PeopleGroup (g);
					pgs.Add (g, pg);
				}

				pg.People.Add (p);
			}

			GroupedPeople = pgs.Values.OrderBy (x => x.Title).ToList ();
		}

		#endregion

		#region Events

		public event EventHandler<ErrorEventArgs> Error;

		public event EventHandler SearchCompleted;

		#endregion
	}


}
